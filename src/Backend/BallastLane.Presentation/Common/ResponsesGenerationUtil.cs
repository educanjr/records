
using BallastLane.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BallastLane.Presentation.Common;

internal static class ResponsesGenerationUtil
{
    public static ProblemDetails CreateProblemDetails(
        string title,
        string type,
        string detail,
        int status,
        Error[]? errors = null) => new()
        {
            Title = title,
            Type = type,
            Detail = detail,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
}
