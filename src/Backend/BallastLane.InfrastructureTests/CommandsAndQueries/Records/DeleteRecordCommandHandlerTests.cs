
using BallastLane.Application.CommandAndQueries.Records.Delete;
using BallastLane.ApplicationTests.Utils;
using BallastLane.Domain.Common;
using BallastLane.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace BallastLane.ApplicationTests.CommandsAndQueries.Records;

public class DeleteRecordCommandHandlerTests
{
    private readonly Guid ValidId = Guid.NewGuid();
    private readonly Guid ValidCreatorId = Guid.NewGuid();

    private readonly Mock<IRecordRepository> _recordRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public DeleteRecordCommandHandlerTests()
    {
        _recordRepositoryMock = new();
        _userRepositoryMock = new();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_NonExistingRecord()
    {
        var command = new DeleteRecordCommand(ValidId, ValidCreatorId);

        var handler = new DeleteRecordCommandHandler(_recordRepositoryMock.Object, _userRepositoryMock.Object);

        DomainResult result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_NonExistingAuthor()
    {
        var command = new DeleteRecordCommand(ValidId, ValidCreatorId);

        _recordRepositoryMock.Setup(
            x => x.Get(It.IsAny<Guid>()))
            .ReturnsAsync(ValueGeneratorUtility.GenerateRecord(ValidId));

        var handler = new DeleteRecordCommandHandler(_recordRepositoryMock.Object, _userRepositoryMock.Object);

        DomainResult result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_DifferentAuthorSupplied()
    {
        var command = new DeleteRecordCommand(ValidId, ValidCreatorId);

        _recordRepositoryMock.Setup(
            x => x.Get(It.IsAny<Guid>()))
            .ReturnsAsync(ValueGeneratorUtility.GenerateRecord(ValidId));

        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ValueGeneratorUtility.GenerateUser(ValidCreatorId));

        var handler = new DeleteRecordCommandHandler(_recordRepositoryMock.Object, _userRepositoryMock.Object);

        DomainResult result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Success()
    {
        var command = new DeleteRecordCommand(ValidId, ValidCreatorId);

        _recordRepositoryMock.Setup(
            x => x.Get(It.IsAny<Guid>()))
            .ReturnsAsync(ValueGeneratorUtility.GenerateRecord(ValidId, ValidCreatorId));

        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ValueGeneratorUtility.GenerateUser(ValidCreatorId));

        var handler = new DeleteRecordCommandHandler(_recordRepositoryMock.Object, _userRepositoryMock.Object);

        DomainResult result = await handler.Handle(command, default);

        _recordRepositoryMock.Verify(
            x => x.Delete(It.Is<Guid>(i => i.ToString() == ValidId.ToString()), It.Is<Guid>(i => i.ToString() == ValidCreatorId.ToString())),
            Times.Once);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
    }
}
