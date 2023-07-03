
using BallastLane.Application.CommandAndQueries.Users.Update;
using BallastLane.Domain.Common;
using BallastLane.Presentation.Contracts;
using BallastLane.Presentation.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace BallastLane.PresentationTests.Controllers.Users;

public class UpdateUserTests
{
    private const string ValidFirstName = "Firstname";
    private const string ValidLastName = "Last Name";
    private static readonly Guid ValidId = Guid.NewGuid();

    private readonly Mock<ISender> _senderMock;


    public UpdateUserTests()
    {
        _senderMock = new();
    }

    [Fact]
    public async Task Api_Should_Return_BadRequest_On_HandlerFailure()
    {
        UpdateUserRequest request = new(ValidFirstName, ValidLastName);

        _senderMock.Setup(
            x => x.Send(
                It.IsAny<UpdateUserCommand>(),
                default))
            .ReturnsAsync(DomainResult.Failure(It.IsAny<Error>()));

        var controller = new ControllerMock(_senderMock.Object);

        IActionResult result = await controller.UpdateUser(ValidId, request, default);

        BadRequestObjectResult? concreteResult = result as BadRequestObjectResult;
        concreteResult.Should().NotBeNull();
        concreteResult.Value.Should().NotBeNull();
        concreteResult.Value.Should().BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task Api_Should_Return_Forbiden_On_InvalidUserId()
    {
        UpdateUserRequest request = new(ValidFirstName, ValidLastName);

        _senderMock.Setup(
            x => x.Send(
                It.IsAny<UpdateUserCommand>(),
                default))
            .ReturnsAsync(DomainResult.Failure(It.IsAny<Error>()));

        var controller = new ControllerMock(_senderMock.Object);

        IActionResult result = await controller.UpdateUser(Guid.NewGuid(), request, default);

        ForbidResult? concreteResult = result as ForbidResult;
        concreteResult.Should().NotBeNull();
        concreteResult.AuthenticationSchemes.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Api_Should_Return_OK_On_HandlerSuccess()
    {
        UpdateUserRequest request = new(ValidFirstName, ValidLastName);

        _senderMock.Setup(
            x => x.Send(
                It.IsAny<UpdateUserCommand>(),
                default))
            .ReturnsAsync(DomainResult.Success());

        var controller = new ControllerMock(_senderMock.Object);

        IActionResult result = await controller.UpdateUser(ValidId, request, default);

        NoContentResult? concreteResult = result as NoContentResult;
        concreteResult.Should().NotBeNull();
        concreteResult.StatusCode.Should().Be((int)StatusCodes.Status204NoContent);
    }

    internal sealed class ControllerMock : UsersController
    {
        public ControllerMock(ISender sender) : base(sender)
        {
        }

        protected override string AuthenticatedUserId => ValidId.ToString();
    }
}
