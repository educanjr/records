
using BallastLane.Application.Abstractions;
using BallastLane.Application.CommandAndQueries.Users.Create;
using BallastLane.Domain.Common;
using BallastLane.Domain.Entities;
using BallastLane.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace BallastLane.ApplicationTests.CommandsAndQueries.Users;

public class CreateUserCommandHandlerTests
{
    private const string ValidFirstName = "Firstname";
    private const string ValidLastName = "Last Name";
    private const string ValidEmail = "testc@email.com";
    private const string ValidPassword = "password";
    private readonly Guid ValidId = Guid.NewGuid();

    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordProvider> _passwordProviderMock;

    public CreateUserCommandHandlerTests()
    {
        _passwordProviderMock = new();
        _userRepositoryMock = new();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_InvalidUserCreation()
    {
        var command = new CreateUserCommand(string.Empty, string.Empty, string.Empty, ValidPassword);

        var handler = new CreateUserCommandHandler(_userRepositoryMock.Object, _passwordProviderMock.Object);

        DomainResult<Guid> result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_DuplicatedEmail()
    {
        var command = new CreateUserCommand(string.Empty, string.Empty, string.Empty, ValidPassword);

        _userRepositoryMock.Setup(
            x => x.IsEmailUniqueAsync(
                It.IsAny<string>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new CreateUserCommandHandler(_userRepositoryMock.Object, _passwordProviderMock.Object);

        DomainResult<Guid> result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Success()
    {
        var command = new CreateUserCommand(ValidEmail, ValidFirstName, ValidLastName, ValidPassword);

        _userRepositoryMock.Setup(
            x => x.IsEmailUniqueAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _passwordProviderMock.Setup(
            x => x.HashPassword(
                It.IsAny<string>()))
            .Returns(ValidPassword);

        var handler = new CreateUserCommandHandler(_userRepositoryMock.Object, _passwordProviderMock.Object);

        DomainResult<Guid> result = await handler.Handle(command, default);

        _passwordProviderMock.Verify(
            x => x.HashPassword(It.Is<string>(i => i == ValidPassword)),
            Times.Once);

        _userRepositoryMock.Verify( 
            x => x.Add(It.Is<User>(i => i.Id == result.Value)),
            Times.Once);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }
}
