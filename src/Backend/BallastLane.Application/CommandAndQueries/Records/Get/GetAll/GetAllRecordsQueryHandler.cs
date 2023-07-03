
using BallastLane.Application.Abstractions;
using BallastLane.Domain.Common;
using BallastLane.Domain.Repositories;

namespace BallastLane.Application.CommandAndQueries.Records.Get.GetAll;

internal sealed class GetAllRecordsQueryHandler : IQueryHandler<GetAllRecordsQuery, IEnumerable<RecordResponse>>
{
    private readonly IRecordRepository _recordRepository;

    public GetAllRecordsQueryHandler(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    public async Task<DomainResult<IEnumerable<RecordResponse>>> Handle(GetAllRecordsQuery request, CancellationToken cancellationToken)
    {
        var records = await _recordRepository.GetAll();

        if(records is null || !records.Any())
        {
            return DomainResult.Success(Enumerable.Empty<RecordResponse>());
        }

        var result = records.Select(i => new RecordResponse(
            i.Id,
            i.Title,
            i.Description,
            i.Creator.Id));

        return DomainResult.Success(result);
    }
}
