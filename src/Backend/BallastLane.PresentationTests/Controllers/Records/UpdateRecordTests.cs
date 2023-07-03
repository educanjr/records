
using BallastLane.Application.CommandAndQueries.Records.Update;
using BallastLane.Application.Errors;
using BallastLane.Domain.Common;
using BallastLane.Presentation.Contracts;
using BallastLane.Presentation.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BallastLane.PresentationTests.Controllers.Records;

public class UpdateRecordTests
{
    private const string ValidTitle = "Title";
    private const string ValidDescription = "Description";
    private readonly Guid ValidId = Guid.NewGuid();
    private static readonly Guid ValidIdCreator = Guid.NewGuid();

    private readonly Mock<ISender> _senderMock;

    public UpdateRecordTests()
    {
        _senderMock = new();
    }

    [Fact]
    public async Task Api_Should_Return_Unauthorized_On_InvalidUserId()
    {
        _senderMock.Setup(
            x => x.Send(
                It.IsAny<UpdateRecordCommand>(),
                default))
            .ReturnsAsync(DomainResult.Success());

        var controller = new InvalidControllerMock(_senderMock.Object);

        IActionResult result = await controller.UpdateRecord(ValidId, It.IsAny<UpdateRecordRequest>(), default);

        UnauthorizedObjectResult? concreteResult = result as UnauthorizedObjectResult;
        concreteResult.Should().NotBeNull();
        concreteResult.Value.Should().NotBeNull();
        concreteResult.Value.Should().BeOfType<Error>();
    }

    [Fact]
    public async Task Api_Should_Return_BadRequest_On_HandlerFailure()
    {
        UpdateRecordRequest request = new UpdateRecordRequest(ValidTitle, ValidDescription);

        _senderMock.Setup(
            x => x.Send(
                It.IsAny<UpdateRecordCommand>(),
                default))
            .ReturnsAsync(DomainResult.Failure(RecordErrors.CreatorNotFound));
        
        var controller = new ValidControllerMock(_senderMock.Object);

        IActionResult result = await controller.UpdateRecord(ValidId, request, default);

        BadRequestObjectResult? concreteResult = result as BadRequestObjectResult;
        concreteResult.Should().NotBeNull();
        concreteResult.Value.Should().NotBeNull();
        concreteResult.Value.Should().BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task Api_Should_Return_Created_On_HandlerSuccess()
    {
        UpdateRecordRequest request = new UpdateRecordRequest(ValidTitle, ValidDescription);

        _senderMock.Setup(
            x => x.Send(
                It.IsAny<UpdateRecordCommand>(),
                default))
            .ReturnsAsync(DomainResult.Success());

        var controller = new ValidControllerMock(_senderMock.Object);

        IActionResult result = await controller.UpdateRecord(ValidId, request, default);

        NoContentResult? concreteResult = result as NoContentResult;
        concreteResult.Should().NotBeNull();
        concreteResult.StatusCode.Should().Be((int)StatusCodes.Status204NoContent);
    }

    internal sealed class ValidControllerMock : RecordsController
    {
        public ValidControllerMock(ISender sender) : base(sender)
        {
        }

        protected override string AuthenticatedUserId => ValidIdCreator.ToString();
    }

    internal sealed class InvalidControllerMock : RecordsController
    {
        public InvalidControllerMock(ISender sender) : base(sender)
        {
        }

        protected override string AuthenticatedUserId => "notguid";
    }
}
