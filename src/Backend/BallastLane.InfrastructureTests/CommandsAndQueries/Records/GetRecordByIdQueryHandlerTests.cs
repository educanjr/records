
using BallastLane.Application.CommandAndQueries.Records.Get;
using BallastLane.Application.CommandAndQueries.Records.Get.GetById;
using BallastLane.ApplicationTests.Utils;
using BallastLane.Domain.Common;
using BallastLane.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace BallastLane.ApplicationTests.CommandsAndQueries.Records;

public class GetRecordByIdQueryHandlerTests
{
    private readonly Guid ValidId = Guid.NewGuid();

    private readonly Mock<IRecordRepository> _recordRepositoryMock;

    public GetRecordByIdQueryHandlerTests()
    {
        _recordRepositoryMock = new();
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_On_NonExistingRecord()
    {
        var command = new GetRecordByIdQuery(ValidId);

        var handler = new GetRecordByIdQueryHandler(_recordRepositoryMock.Object);

        DomainResult<RecordResponse> result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Success()
    {
        var command = new GetRecordByIdQuery(ValidId);

        _recordRepositoryMock.Setup(
            x => x.Get(It.IsAny<Guid>()))
            .ReturnsAsync(ValueGeneratorUtility.GenerateRecord(ValidId));

        var handler = new GetRecordByIdQueryHandler(_recordRepositoryMock.Object);

        DomainResult<RecordResponse> result = await handler.Handle(command, default);

        _recordRepositoryMock.Verify(
            x => x.Get(It.Is<Guid>(i => i.ToString() == ValidId.ToString())),
            Times.Once);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeNull();
        result.Value.Id.ToString().Should().Be(ValidId.ToString());
    }
}
