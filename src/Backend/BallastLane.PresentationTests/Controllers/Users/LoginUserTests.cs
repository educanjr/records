
using BallastLane.Application.CommandAndQueries.Users.Login;
using BallastLane.Domain.Common;
using BallastLane.Presentation.Contracts;
using BallastLane.Presentation.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BallastLane.PresentationTests.Controllers.Users;

public class LoginUserTests
{
    private const string ValidFirstName = "Firstname";
    private const string ValidLastName = "Last Name";
    private const string ValidEmail = "testc@email.com";
    private const string ValidPassword = "password";
    private readonly Guid ValidId = Guid.NewGuid();

    private readonly Mock<ISender> _senderMock;

    public LoginUserTests()
    {
        _senderMock = new();
    }

    [Fact]
    public async Task Api_Should_Return_BadRequest_On_HandlerFailure()
    {
        LoginRequest request = new(ValidEmail, ValidPassword);

        _senderMock.Setup(
            x => x.Send(
                It.IsAny<LoginCommand>(),
                default))
            .ReturnsAsync(DomainResult.Failure<string>(It.IsAny<Error>()));

        var controller = new UsersController(_senderMock.Object);

        IActionResult result = await controller.LoginUser(request, default);

        BadRequestObjectResult? concreteResult = result as BadRequestObjectResult;
        concreteResult.Should().NotBeNull();
        concreteResult.Value.Should().NotBeNull();
        concreteResult.Value.Should().BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task Api_Should_Return_OK_On_HandlerSuccess()
    {
        LoginRequest request = new(ValidEmail, ValidPassword);

        _senderMock.Setup(
            x => x.Send(
                It.IsAny<LoginCommand>(),
                default))
            .ReturnsAsync(DomainResult.Success(ValidPassword));

        var controller = new UsersController(_senderMock.Object);

        IActionResult result = await controller.LoginUser(request, default);

        ObjectResult? concreteResult = result as ObjectResult;
        concreteResult.Should().NotBeNull();
        concreteResult.Value.Should().NotBeNull();
        concreteResult.StatusCode.Should().Be((int)StatusCodes.Status200OK);
        concreteResult.Value.Should().BeOfType<string>();
    }
}
