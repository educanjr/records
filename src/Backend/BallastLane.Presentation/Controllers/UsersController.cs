
using BallastLane.Application.CommandAndQueries.Records.Update;
using BallastLane.Application.CommandAndQueries.Users.Create;
using BallastLane.Application.CommandAndQueries.Users.GetById;
using BallastLane.Application.CommandAndQueries.Users.Login;
using BallastLane.Application.CommandAndQueries.Users.Update;
using BallastLane.Domain.Common;
using BallastLane.Presentation.Abstractions;
using BallastLane.Presentation.Contracts;
using BallastLane.Presentation.Errors;
using BallastLane.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BallastLane.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController 
    : BaseApiController
{
    public UsersController(ISender sender)
        : base(sender)
    {
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserByIdResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error[]))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Error))]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var strUserId = AuthenticatedUserId;
        if (!string.IsNullOrWhiteSpace(strUserId) && Guid.TryParse(strUserId, out Guid userId) && id == userId)
        {
            var query = new GetUserByIdQuery(id);

            DomainResult<GetUserByIdResponse> result = await Sender.Send(query, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : NotFound(result.Errors);
        }

        return Forbid(AuthorizationErrors.InvalidPermissions);
    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        return await DomainResult
            .Create(
                new CreateUserCommand(request.Email, request.FirstName, request.LastName, request.Password)
            )
            .Bind(command => Sender.Send(command, cancellationToken))
            .Match(
                id => CreatedAtAction(nameof(GetUserById), new {id}, id),
                HandleFailure
            );
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContent))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Error))]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(
        Guid id,
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var strUserId = AuthenticatedUserId;
        if (!string.IsNullOrWhiteSpace(strUserId) && Guid.TryParse(strUserId, out Guid userId) && id == userId)
        {
            return await DomainResult
                .Create(
                    new UpdateUserCommand(id, request.FirstName, request.LastName)
                )
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(
                    NoContent,
                    HandleFailure
                );
        }

        return Forbid(AuthorizationErrors.InvalidPermissions);
    }


    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(
       [FromBody] LoginRequest request,
       CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);

        DomainResult<string> tokenResult = await Sender.Send(
            command,
            cancellationToken);

        if (tokenResult.IsFailure)
        {
            return HandleFailure(tokenResult);
        }

        return Ok(tokenResult.Value);
    }
}
