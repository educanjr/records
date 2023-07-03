
using BallastLane.Application.Abstractions;

namespace BallastLane.Application.CommandAndQueries.Records.Get.GetById;

public sealed record GetRecordByIdQuery(Guid Id) : IQuery<RecordResponse>;
