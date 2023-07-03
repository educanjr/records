
using BallastLane.Application.CommandAndQueries.Users.Update;
using BallastLane.ApplicationTests.Utils;
using BallastLane.Domain.Common;
using BallastLane.Domain.Entities;
using BallastLane.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace BallastLane.ApplicationTests.CommandsAndQueries.Users;

public class UpdateUserCommandHandlerTests
{
    private const string ValidFirstName = "Firstname";
    private const string ValidLastName = "Last Name";
    private readonly Guid ValidId = Guid.NewGuid();

    private readonly Mock<IUserRepository> _userRepositoryMock;

    public UpdateUserCommandHandlerTests()
    {
        _userRepositoryMock = new();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_NonExistingUser()
    {
        var command = new UpdateUserCommand(ValidId, ValidFirstName, ValidLastName);

        var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object);

        DomainResult result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_InvalidNameChange()
    {
        var command = new UpdateUserCommand(ValidId, string.Empty, string.Empty);

        var user = ValueGeneratorUtility.GenerateUser();
        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object);

        DomainResult result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();

        user.FirstName.Should().NotBeEmpty();
        user.LastName.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Return_Success()
    {
        var command = new UpdateUserCommand(ValidId, ValidFirstName, ValidLastName);

        var user = ValueGeneratorUtility.GenerateUser();
        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object);

        DomainResult result = await handler.Handle(command, default);

        _userRepositoryMock.Verify(
            x => x.GetUserByIdAsync(It.Is<Guid>(i => i.ToString() == ValidId.ToString()), It.IsAny<CancellationToken>()),
            Times.Once);

        _userRepositoryMock.Verify(
            x => x.Update(It.Is<User>(i => i.Id.ToString() == user.Id.ToString())),
            Times.Once);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
    }
}
