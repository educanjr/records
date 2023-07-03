
using BallastLane.Application.Abstractions;
using BallastLane.Application.CommandAndQueries.Records.Create;
using BallastLane.Application.CommandAndQueries.Users.Create;
using BallastLane.ApplicationTests.Utils;
using BallastLane.Domain.Common;
using BallastLane.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace BallastLane.ApplicationTests.CommandsAndQueries.Records;

public class CreateRecordCommandHandlerTests
{
    private const string ValidDescription = "Valid Description";
    private const string ValidTitle = "Valid Title";
    private readonly Guid ValidId = Guid.NewGuid();

    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IRecordRepository> _recordRepositoryMock;

    public CreateRecordCommandHandlerTests()
    {
        _recordRepositoryMock = new();
        _userRepositoryMock = new();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_NonExistingUser()
    {
        var command = new CreateRecordCommand(ValidId, ValidTitle, ValidDescription);

        var handler = new CreateRecordCommandHandler(_userRepositoryMock.Object, _recordRepositoryMock.Object);

        DomainResult<Guid> result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_InvalidRecordCreation()
    {
        var command = new CreateRecordCommand(ValidId, string.Empty, string.Empty);

        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ValueGeneratorUtility.GenerateUser());

        var handler = new CreateRecordCommandHandler(_userRepositoryMock.Object, _recordRepositoryMock.Object);

        DomainResult<Guid> result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Success()
    {
        var command = new CreateRecordCommand(ValidId, ValidTitle, ValidDescription);

        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ValueGeneratorUtility.GenerateUser());

        var handler = new CreateRecordCommandHandler(_userRepositoryMock.Object, _recordRepositoryMock.Object);

        DomainResult<Guid> result = await handler.Handle(command, default);

        _recordRepositoryMock.Verify(
            x => x.Add(It.Is<Domain.Entities.Record>(i => i.Id.ToString() == result.Value.ToString())),
            Times.Once);

        _userRepositoryMock.Verify(
            x => x.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }
}
