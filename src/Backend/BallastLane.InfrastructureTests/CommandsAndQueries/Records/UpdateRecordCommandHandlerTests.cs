
using BallastLane.Application.CommandAndQueries.Records.Update;
using BallastLane.ApplicationTests.Utils;
using BallastLane.Domain.Common;
using BallastLane.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace BallastLane.ApplicationTests.CommandsAndQueries.Records;

public class UpdateRecordCommandHandlerTests
{
    private const string ValidDescription = "Valid Description";
    private const string ValidTitle = "Valid Title";
    private readonly Guid ValidId = Guid.NewGuid();
    private readonly Guid ValidCreatorId = Guid.NewGuid();

    private readonly Mock<IRecordRepository> _recordRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public UpdateRecordCommandHandlerTests()
    {
        _recordRepositoryMock = new();
        _userRepositoryMock = new();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_NonExistingRecord()
    {
        var command = new UpdateRecordCommand(ValidId, ValidTitle, ValidDescription, ValidCreatorId);

        var handler = new UpdateRecordCommandHandler(_recordRepositoryMock.Object, _userRepositoryMock.Object);

        DomainResult result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_NonExistingAuthor()
    {
        var command = new UpdateRecordCommand(ValidId, ValidTitle, ValidDescription, ValidCreatorId);

        _recordRepositoryMock.Setup(
            x => x.Get(It.IsAny<Guid>()))
            .ReturnsAsync(ValueGeneratorUtility.GenerateRecord(ValidId));

        var handler = new UpdateRecordCommandHandler(_recordRepositoryMock.Object, _userRepositoryMock.Object);

        DomainResult result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_DifferentAuthorSupplied()
    {
        var command = new UpdateRecordCommand(ValidId, ValidTitle, ValidDescription, ValidCreatorId);

        _recordRepositoryMock.Setup(
            x => x.Get(It.IsAny<Guid>()))
            .ReturnsAsync(ValueGeneratorUtility.GenerateRecord(ValidId));

        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ValueGeneratorUtility.GenerateUser(ValidCreatorId));

        var handler = new UpdateRecordCommandHandler(_recordRepositoryMock.Object, _userRepositoryMock.Object);

        DomainResult result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public async Task Handle_Should_Return_Failure_On_InvalidPropertiesUpdate(bool isTitleValid, bool isDescriptionValid)
    {
        var title = isTitleValid ? ValidTitle : string.Empty;
        var description = isDescriptionValid ? ValidDescription : string.Empty;

        var command = new UpdateRecordCommand(ValidId, title, description, ValidCreatorId);

        _recordRepositoryMock.Setup(
            x => x.Get(It.IsAny<Guid>()))
            .ReturnsAsync(ValueGeneratorUtility.GenerateRecord(ValidId, ValidCreatorId));

        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ValueGeneratorUtility.GenerateUser(ValidCreatorId));

        var handler = new UpdateRecordCommandHandler(_recordRepositoryMock.Object, _userRepositoryMock.Object);

        DomainResult result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Success()
    {
        var command = new UpdateRecordCommand(ValidId, ValidTitle, ValidDescription, ValidCreatorId);

        _recordRepositoryMock.Setup(
            x => x.Get(It.IsAny<Guid>()))
            .ReturnsAsync(ValueGeneratorUtility.GenerateRecord(ValidId, ValidCreatorId));

        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ValueGeneratorUtility.GenerateUser(ValidCreatorId));

        var handler = new UpdateRecordCommandHandler(_recordRepositoryMock.Object, _userRepositoryMock.Object);

        DomainResult result = await handler.Handle(command, default);

        _recordRepositoryMock.Verify(
            x => x.Update(It.Is<Domain.Entities.Record>(i => i.Id.ToString() == ValidId.ToString())),
            Times.Once);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
    }
}
