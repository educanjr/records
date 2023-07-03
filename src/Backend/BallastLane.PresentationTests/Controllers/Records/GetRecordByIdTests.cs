
using BallastLane.Application.CommandAndQueries.Records.Get;
using BallastLane.Application.CommandAndQueries.Records.Get.GetById;
using BallastLane.Application.CommandAndQueries.Users.GetById;
using BallastLane.Application.Errors;
using BallastLane.Domain.Common;
using BallastLane.Presentation.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BallastLane.PresentationTests.Controllers.Records;

public class GetRecordByIdTests
{
    private const string ValidTitle = "Title";
    private const string ValidDescription = "Description";
    private readonly Guid ValidId = Guid.NewGuid();
    private readonly Guid ValidIdCreator = Guid.NewGuid();

    private readonly Mock<ISender> _senderMock;

    public GetRecordByIdTests()
    {
        _senderMock = new();
    }

    [Fact]
    public async Task Api_Should_Return_NotFound_On_HandlerFailure()
    {
        _senderMock.Setup(
            x => x.Send(
                It.IsAny<GetRecordByIdQuery>(),
                default))
            .ReturnsAsync(DomainResult.Failure<RecordResponse>(RecordErrors.NotFound(ValidId)));

        var controller = new RecordsController(_senderMock.Object);

        IActionResult result = await controller.GetRecordById(ValidId, default);

        NotFoundObjectResult? concreteResult = result as NotFoundObjectResult;
        concreteResult.Should().NotBeNull();
        concreteResult.Value.Should().NotBeNull();
        concreteResult.Value.Should().BeOfType<Error[]>();
    }

    [Fact]
    public async Task Api_Should_Return_OK_On_HandlerSuccess()
    {
        RecordResponse response = new(ValidId, ValidTitle, ValidDescription, ValidIdCreator);

        _senderMock.Setup(
            x => x.Send(
                It.IsAny<GetRecordByIdQuery>(),
                default))
            .ReturnsAsync(DomainResult.Success(response));

        var controller = new RecordsController(_senderMock.Object);

        IActionResult result = await controller.GetRecordById(ValidId, default);

        OkObjectResult? concreteResult = result as OkObjectResult;
        concreteResult.Should().NotBeNull();
        concreteResult.Value.Should().NotBeNull();
        concreteResult.Value.Should().BeOfType<RecordResponse>();
    }
}
