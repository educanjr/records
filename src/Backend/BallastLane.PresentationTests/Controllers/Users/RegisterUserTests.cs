
using BallastLane.Application.CommandAndQueries.Users.Create;
using BallastLane.Application.CommandAndQueries.Users.GetById;
using BallastLane.Domain.Common;
using BallastLane.Presentation.Contracts;
using BallastLane.Presentation.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BallastLane.PresentationTests.Controllers.Users;

public class RegisterUserTests
{
    private const string ValidFirstName = "Firstname";
    private const string ValidLastName = "Last Name";
    private const string ValidEmail = "testc@email.com";
    private const string ValidPassword = "password";
    private readonly Guid ValidId = Guid.NewGuid();

    private readonly Mock<ISender> _senderMock;

    public RegisterUserTests()
    {
        _senderMock = new();
    }

    [Fact]
    public async Task Api_Should_Return_BadRequest_On_HandlerFailure()
    {
        RegisterUserRequest request = new(ValidEmail, ValidFirstName, ValidLastName, ValidPassword);
        _senderMock.Setup(
            x => x.Send(
                It.IsAny<CreateUserCommand>(),
                default))
            .ReturnsAsync(DomainResult.Failure<Guid>(It.IsAny<Error>()));

        var controller = new UsersController(_senderMock.Object);

        IActionResult result = await controller.RegisterUser(request, default);

        BadRequestObjectResult? concreteResult = result as BadRequestObjectResult;
        concreteResult.Should().NotBeNull();
        concreteResult.Value.Should().NotBeNull();
        concreteResult.Value.Should().BeOfType<ProblemDetails>();
    }

    [Fact]
    public async Task Api_Should_Return_OK_On_HandlerSuccess()
    {
        RegisterUserRequest request = new(ValidEmail, ValidFirstName, ValidLastName, ValidPassword);
        _senderMock.Setup(
            x => x.Send(
                It.IsAny<CreateUserCommand>(),
                default))
            .ReturnsAsync(DomainResult.Success(ValidId));

        var controller = new UsersController(_senderMock.Object);

        IActionResult result = await controller.RegisterUser(request, default);

        ObjectResult? concreteResult = result as ObjectResult;
        concreteResult.Should().NotBeNull();
        concreteResult.Value.Should().NotBeNull();
        concreteResult.StatusCode.Should().Be((int)StatusCodes.Status201Created);
        concreteResult.Value.Should().BeOfType<Guid>();
    }
}
