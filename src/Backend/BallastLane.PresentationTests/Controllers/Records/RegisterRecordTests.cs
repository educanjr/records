
using BallastLane.Application.CommandAndQueries.Records.Create;
using BallastLane.Application.CommandAndQueries.Users.GetById;
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

public class RegisterRecordTests
{
    private const string ValidTitle = "Title";
    private const string ValidDescription = "Description";
    private readonly Guid ValidId = Guid.NewGuid();
    private static readonly Guid ValidIdCreator = Guid.NewGuid();

    private readonly Mock<ISender> _senderMock;

    public RegisterRecordTests()
    {
        _senderMock = new();
    }

    [Fact]
    public async Task Api_Should_Return_Forbiden_On_InvalidUserId()
    {
        RegisterRecordRequest request = new RegisterRecordRequest(ValidTitle, ValidDescription, Guid.NewGuid());

        _senderMock.Setup(
            x => x.Send(
                It.IsAny<CreateRecordCommand>(),
                default))
            .ReturnsAsync(DomainResult.Success(It.IsAny<Guid>()));

        var controller = new ControllerMock(_senderMock.Object);

        IActionResult result = await controller.RegisterRecord(request, default);

        ForbidResult? concreteResult = result as ForbidResult;
        concreteResult.Should().NotBeNull();
        concreteResult.AuthenticationSchemes.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Api_Should_Return_BadRequest_On_HandlerFailure()
    {
        RegisterRecordRequest request = new RegisterRecordRequest(ValidTitle, ValidDescription, ValidIdCreator);

        _senderMock.Setup(
            x => x.Send(
                It.IsAny<CreateRecordCommand>(),
                default))
            .ReturnsAsync(DomainResult.Failure<Guid>(RecordErrors.CreatorNotFound));

        var controller = new ControllerMock(_senderMock.Object);

        IActionResult result = await controller.RegisterRecord(request, default);

        BadRequestObjectResult? concreteResult = result as BadRequestObjectResult;
        concreteResult.Should().NotBeNull();
        concreteResult.Value.Should().NotBeNull();
        concreteResult.Value.Should().BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task Api_Should_Return_Created_On_HandlerSuccess()
    {
        RegisterRecordRequest request = new RegisterRecordRequest(ValidTitle, ValidDescription, ValidIdCreator);

        _senderMock.Setup(
            x => x.Send(
                It.IsAny<CreateRecordCommand>(),
                default))
            .ReturnsAsync(DomainResult.Success(ValidId));

        var controller = new ControllerMock(_senderMock.Object);

        IActionResult result = await controller.RegisterRecord(request, default);

        ObjectResult? concreteResult = result as ObjectResult;
        concreteResult.Should().NotBeNull();
        concreteResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        concreteResult.Value.Should().NotBeNull();
        concreteResult.Value.Should().BeOfType<Guid>();
    }

    internal sealed class ControllerMock : RecordsController
    {
        public ControllerMock(ISender sender) : base(sender)
        {
        }

        protected override string AuthenticatedUserId => ValidIdCreator.ToString();
    }
}
