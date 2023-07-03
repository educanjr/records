
using BallastLane.Application.Abstractions;
using BallastLane.Application.CommandAndQueries.Users.Login;
using BallastLane.ApplicationTests.Utils;
using BallastLane.Domain.Common;
using BallastLane.Domain.Entities;
using BallastLane.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace BallastLane.ApplicationTests.CommandsAndQueries.Users;

public class LoginCommandHandlerTests
{
    private const string ValidEmail = "testc@email.com";
    private const string ValidPassword = "password";
    private readonly Guid ValidId = Guid.NewGuid();

    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordProvider> _passwordProviderMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;

    public LoginCommandHandlerTests()
    {
        _userRepositoryMock = new();
        _passwordProviderMock = new();
        _jwtProviderMock = new();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_NonExistingUser()
    {
        var command = new LoginCommand(ValidEmail, ValidPassword);

        var handler = new LoginCommandHandler(_userRepositoryMock.Object, _jwtProviderMock.Object, _passwordProviderMock.Object);

        DomainResult<string> result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_DifferentPassword()
    {
        var command = new LoginCommand(ValidEmail, ValidPassword);

        var user = ValueGeneratorUtility.GenerateUser();
        _userRepositoryMock.Setup(
            x => x.GetUserByEmailAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordProviderMock.Setup(
            x => x.ComparePasswords(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(false);

        var handler = new LoginCommandHandler(_userRepositoryMock.Object, _jwtProviderMock.Object, _passwordProviderMock.Object);

        DomainResult<string> result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Success()
    {
        var command = new LoginCommand(ValidEmail, ValidPassword);

        var user = ValueGeneratorUtility.GenerateUser();
        _userRepositoryMock.Setup(
            x => x.GetUserByEmailAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordProviderMock.Setup(
            x => x.ComparePasswords(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(true);

        var token = ValueGeneratorUtility.GenerateString(20);
        _jwtProviderMock.Setup(
            x => x.Generate(
                It.IsAny<User>()))
            .Returns(token);

        var handler = new LoginCommandHandler(_userRepositoryMock.Object, _jwtProviderMock.Object, _passwordProviderMock.Object);

        DomainResult<string> result = await handler.Handle(command, default);

        _userRepositoryMock.Verify(
            x => x.GetUserByEmailAsync(It.Is<string>(i => i == ValidEmail), It.IsAny<CancellationToken>()),
            Times.Once);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeEmpty();
        result.Value.Should().Be(token);
    }
}
