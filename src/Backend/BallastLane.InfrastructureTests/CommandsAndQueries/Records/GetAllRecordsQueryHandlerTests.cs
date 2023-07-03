
using BallastLane.Application.CommandAndQueries.Records.Get;
using BallastLane.Application.CommandAndQueries.Records.Get.GetAll;
using BallastLane.ApplicationTests.Utils;
using BallastLane.Domain.Common;
using BallastLane.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace BallastLane.ApplicationTests.CommandsAndQueries.Records;

public class GetAllRecordsQueryHandlerTests
{
    private readonly Mock<IRecordRepository> _recordRepositoryMock;

    public GetAllRecordsQueryHandlerTests()
    {
        _recordRepositoryMock = new();
    }

    [Fact]
    public async Task Handle_Should_Return_SuccessEmptyList_On_NonExistingRecords()
    {
        var command = new GetAllRecordsQuery();

        var handler = new GetAllRecordsQueryHandler(_recordRepositoryMock.Object);

        DomainResult<IEnumerable<RecordResponse>> result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeNull();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Return_Success()
    {
        var command = new GetAllRecordsQuery();

        var record1 = ValueGeneratorUtility.GenerateRecord();
        var record2 = ValueGeneratorUtility.GenerateRecord();
        _recordRepositoryMock.Setup(
            x => x.GetAll())
            .ReturnsAsync(new[]
            {
                record1,
                record2
            });

        var handler = new GetAllRecordsQueryHandler(_recordRepositoryMock.Object);

        DomainResult<IEnumerable<RecordResponse>> result = await handler.Handle(command, default);

        _recordRepositoryMock.Verify(
            x => x.GetAll(),
            Times.Once);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeNull();
        result.Value.Should().NotBeEmpty();
    }
}
