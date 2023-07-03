
using BallastLane.Application.Abstractions;
using BallastLane.Application.CommandAndQueries.Users.Create;
using BallastLane.Application.CommandAndQueries.Users.GetById;
using BallastLane.ApplicationTests.Utils;
using BallastLane.Domain.Common;
using BallastLane.Domain.Entities;
using BallastLane.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace BallastLane.ApplicationTests.CommandsAndQueries.Users;

public class GetUserByIdQueryHandlerTests
{
    private readonly Guid ValidId = Guid.NewGuid();

    private readonly Mock<IUserRepository> _userRepositoryMock;

    public GetUserByIdQueryHandlerTests()
    {
        _userRepositoryMock = new();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_NonExistingUser()
    {
        var command = new GetUserByIdQuery(ValidId);

        var handler = new GetUserByIdQueryHandler(_userRepositoryMock.Object);

        DomainResult<GetUserByIdResponse> result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Success()
    {
        var command = new GetUserByIdQuery(ValidId);

        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ValueGeneratorUtility.GenerateUser);

        var handler = new GetUserByIdQueryHandler(_userRepositoryMock.Object);

        DomainResult<GetUserByIdResponse> result = await handler.Handle(command, default);

        _userRepositoryMock.Verify(
            x => x.GetUserByIdAsync(It.Is<Guid>(i => i.ToString() == ValidId.ToString()), It.IsAny<CancellationToken>()),
            Times.Once);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}
