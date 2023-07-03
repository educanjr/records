
using BallastLane.Presentation.Common;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace BallastLane.Presentation.Middleware;

public class ExeptionProcessMiddleware
{
    private readonly RequestDelegate _next;

    public ExeptionProcessMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception is InvalidOperationException 
            ? HttpStatusCode.BadRequest
            : HttpStatusCode.InternalServerError;

        var response = ResponsesGenerationUtil.CreateProblemDetails(
                    "A problem was found",
                    statusCode.ToString(),
                    exception.Message,
                    (int)statusCode);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.Status!.Value;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
