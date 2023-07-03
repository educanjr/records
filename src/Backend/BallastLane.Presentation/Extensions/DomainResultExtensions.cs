
using BallastLane.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BallastLane.Presentation.Extensions;

internal static class DomainResultExtensions
{
    internal static async Task<IActionResult> Match(
        this Task<DomainResult> resultTask,
        Func<IActionResult> onSuccess,
        Func<DomainResult, IActionResult> onFailure)
    {
        DomainResult result = await resultTask;
        return result.IsSuccess ? onSuccess() : onFailure(result);
    }

    internal static async Task<IActionResult> Match<TIn>(
        this Task<DomainResult<TIn>> resultTask,
        Func<TIn, IActionResult> onSuccess,
        Func<DomainResult, IActionResult> onFailure)
    {
        DomainResult<TIn> result = await resultTask;
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
    }
}
