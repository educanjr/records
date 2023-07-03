
using BallastLane.Application.CommandAndQueries.Records.Create;
using BallastLane.Application.CommandAndQueries.Records.Delete;
using BallastLane.Application.CommandAndQueries.Records.Get;
using BallastLane.Application.CommandAndQueries.Records.Get.GetAll;
using BallastLane.Application.CommandAndQueries.Records.Get.GetById;
using BallastLane.Application.CommandAndQueries.Records.Update;
using BallastLane.Application.CommandAndQueries.Users.GetById;
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

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RecordsController :
    BaseApiController
{
    public RecordsController(ISender sender)
        : base(sender)
    {
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecordResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error[]))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRecordById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetRecordByIdQuery(id);

        DomainResult<RecordResponse> result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Errors);
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecordResponse[]))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    [HttpGet()]
    public async Task<IActionResult> GetAllRecords(CancellationToken cancellationToken)
    {
        var query = new GetAllRecordsQuery();

        DomainResult<IEnumerable<RecordResponse>> result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : HandleFailure(result);
    }

    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPost()]
    public async Task<IActionResult> RegisterRecord(
        [FromBody] RegisterRecordRequest request,
        CancellationToken cancellationToken)
    {
        var strUserId = AuthenticatedUserId;
        if (!string.IsNullOrWhiteSpace(strUserId) && Guid.TryParse(strUserId, out Guid userId) && userId == request.CreatorId)
        {
            return await DomainResult
            .Create(
                new CreateRecordCommand(request.CreatorId, request.Title, request.Description)
            )
            .Bind(command => Sender.Send(command, cancellationToken))
            .Match(
                id => CreatedAtAction(nameof(GetRecordById), new { id }, id),
                HandleFailure
            );
        }

        return Forbid(AuthorizationErrors.InvalidPermissions);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContent))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Error))]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRecord(
        Guid id,
        [FromBody] UpdateRecordRequest request,
        CancellationToken cancellationToken)
    {
        var strUserId = AuthenticatedUserId;
        if (!string.IsNullOrWhiteSpace(strUserId) && Guid.TryParse(strUserId, out Guid userId))
        {
            return await DomainResult
            .Create(
                new UpdateRecordCommand(id, request.Title, request.Description, userId)
            )
            .Bind(command => Sender.Send(command, cancellationToken))
            .Match(
                NoContent,
                HandleFailure
            );
        }

        return Unauthorized(AuthorizationErrors.UnregistredUser);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContent))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Error))]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRecord(
        Guid id,
        CancellationToken cancellationToken)
    {
        var strUserId = AuthenticatedUserId;
        if (!string.IsNullOrWhiteSpace(strUserId) && Guid.TryParse(strUserId, out Guid userId))
        {

            return await DomainResult
            .Create(
                new DeleteRecordCommand(id, userId)
            )
            .Bind(command => Sender.Send(command, cancellationToken))
            .Match(
                NoContent,
                HandleFailure
            );
        }

        return Unauthorized(AuthorizationErrors.UnregistredUser);
    }
}
