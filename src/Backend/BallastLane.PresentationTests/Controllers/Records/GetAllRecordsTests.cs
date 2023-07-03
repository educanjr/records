
using BallastLane.Application.CommandAndQueries.Records.Get;
using BallastLane.Application.CommandAndQueries.Records.Get.GetAll;
using BallastLane.Application.Errors;
using BallastLane.Domain.Common;
using BallastLane.Presentation.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BallastLane.PresentationTests.Controllers.Records;

public class GetAllRecordsTests
{
    private const string ValidTitle = "Title";
    private const string ValidDescription = "Description";
    private readonly Guid ValidId = Guid.NewGuid();
    private readonly Guid ValidIdCreator = Guid.NewGuid();

    private readonly Mock<ISender> _senderMock;

    public GetAllRecordsTests()
    {
        _senderMock = new();
    }

    [Fact]
    public async Task Api_Should_Return_BadRequest_On_HandlerFailure()
    {
        _senderMock.Setup(
            x => x.Send(
                It.IsAny<GetAllRecordsQuery>(),
                default))
            .ReturnsAsync(DomainResult.Failure<IEnumerable<RecordResponse>>(RecordErrors.NotFound(ValidId)));

        var controller = new RecordsController(_senderMock.Object);

        IActionResult result = await controller.GetAllRecords(default);

        BadRequestObjectResult? concreteResult = result as BadRequestObjectResult;
        concreteResult.Should().NotBeNull();
        concreteResult.Value.Should().NotBeNull();
        concreteResult.Value.Should().BeOfType<ProblemDetails>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Api_Should_Return_OK_On_HandlerSuccess(bool getEmptyList)
    {
        IEnumerable<RecordResponse> response = getEmptyList 
            ? Enumerable.Empty<RecordResponse>()
            : new RecordResponse[]
            {
                new(ValidId, ValidTitle, ValidDescription, ValidIdCreator)
            };

        _senderMock.Setup(
            x => x.Send(
                It.IsAny<GetAllRecordsQuery>(),
                default))
            .ReturnsAsync(DomainResult.Success(response));

        var controller = new RecordsController(_senderMock.Object);

        IActionResult result = await controller.GetAllRecords(default);

        OkObjectResult? concreteResult = result as OkObjectResult;
        concreteResult.Should().NotBeNull();

        IEnumerable<RecordResponse> value = concreteResult.Value as IEnumerable<RecordResponse>;
        value.Should().NotBeNull();
        if (getEmptyList)
        {
            value.Should().BeEmpty();
        }
        else
        {
            value.Should().NotBeEmpty();
        }
    }
}
