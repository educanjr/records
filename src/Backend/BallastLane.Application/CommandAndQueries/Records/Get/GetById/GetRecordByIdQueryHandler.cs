
using BallastLane.Application.Abstractions;
using BallastLane.Application.CommandAndQueries.Users.GetById;
using BallastLane.Application.Errors;
using BallastLane.Domain.Common;
using BallastLane.Domain.Entities;
using BallastLane.Domain.Repositories;

namespace BallastLane.Application.CommandAndQueries.Records.Get.GetById;

internal sealed class GetRecordByIdQueryHandler : IQueryHandler<GetRecordByIdQuery, RecordResponse>
{
    private readonly IRecordRepository _recordRepository;

    public GetRecordByIdQueryHandler(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    public async Task<DomainResult<RecordResponse>> Handle(GetRecordByIdQuery request, CancellationToken cancellationToken)
    {
        var record = await _recordRepository.Get(request.Id);
        if (record is null)
        {
            return DomainResult.Failure<RecordResponse>(RecordErrors.NotFound(request.Id));
        }

        var response = new RecordResponse(record.Id, record.Title, record.Description, record.Creator.Id);
        return DomainResult.Success(response);
    }
}
