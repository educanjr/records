using BallastLane.Domain.Common;
using BallastLane.Presentation.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BallastLane.Presentation.Abstractions;

public abstract class BaseApiController : ControllerBase
{
    private readonly ISender _sender;

    protected ISender Sender => _sender;

    protected virtual string? AuthenticatedUserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    protected BaseApiController(ISender sender) => _sender = sender;

    protected IActionResult HandleFailure(DomainResult result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            _ => BadRequest(
                ResponsesGenerationUtil.CreateProblemDetails(
                    "Bad Request",
                    Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest.ToString(),
                    "Bad Request",
                    Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest,
                    result.Errors
            ))
        };
}
