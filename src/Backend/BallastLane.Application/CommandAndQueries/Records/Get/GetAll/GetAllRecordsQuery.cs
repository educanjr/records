
using BallastLane.Application.Abstractions;

namespace BallastLane.Application.CommandAndQueries.Records.Get.GetAll;

public sealed record GetAllRecordsQuery() : IQuery<IEnumerable<RecordResponse>>;
