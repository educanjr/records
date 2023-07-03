
using BallastLane.Application.CommandAndQueries.Users.GetById;
using BallastLane.Application.Errors;
using BallastLane.Domain.Common;
using BallastLane.Presentation.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BallastLane.PresentationTests.Controllers.Users;

public class GetUserByIdTests
{
    private const string ValidEmail = "testc@email.com";
    private static readonly Guid ValidId = Guid.NewGuid();

    private readonly Mock<ISender> _senderMock;

    public GetUserByIdTests()
    {
        _senderMock = new();
    }

    [Fact]
    public async Task Api_Should_Return_Forbiden_On_InvalidUserId()
    {
        _senderMock.Setup(
            x => x.Send(
                It.IsAny<GetUserByIdQuery>(),
                default))
            .ReturnsAsync(DomainResult.Success(It.IsAny<GetUserByIdResponse>()));

        var controller = new ControllerMock(_senderMock.Object);

        IActionResult result = await controller.GetUserById(Guid.NewGuid(), default);

        ForbidResult? concreteResult = result as ForbidResult;
        concreteResult.Should().NotBeNull();
        concreteResult.AuthenticationSchemes.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Api_Should_Return_NotFound_On_HandlerFailure()
    {
        _senderMock.Setup(
            x => x.Send(
                It.IsAny<GetUserByIdQuery>(), 
                default))
            .ReturnsAsync(DomainResult.Failure<GetUserByIdResponse>(UserErrors.NotFound(ValidId)));

        var controller = new ControllerMock(_senderMock.Object);

        IActionResult result = await controller.GetUserById(ValidId, default);

        NotFoundObjectResult? concreteResult = result as NotFoundObjectResult;
        concreteResult.Should().NotBeNull();
        concreteResult.Value.Should().NotBeNull();
        concreteResult.Value.Should().BeOfType<Error[]>();
    }

    [Fact]
    public async Task Api_Should_Return_OK_On_HandlerSuccess()
    {
        GetUserByIdResponse response = new(ValidId, ValidEmail);
        _senderMock.Setup(
            x => x.Send(
                It.IsAny<GetUserByIdQuery>(),
                default))
            .ReturnsAsync(DomainResult.Success(response));

        var controller = new ControllerMock(_senderMock.Object);

        IActionResult result = await controller.GetUserById(ValidId, default);

        OkObjectResult? concreteResult = result as OkObjectResult;
        concreteResult.Should().NotBeNull();
        concreteResult.Value.Should().NotBeNull();
        concreteResult.Value.Should().BeOfType<GetUserByIdResponse>();
    }

    internal sealed class ControllerMock : UsersController
    {
        public ControllerMock(ISender sender) : base(sender)
        {
        }

        protected override string AuthenticatedUserId => ValidId.ToString();
    }
}
