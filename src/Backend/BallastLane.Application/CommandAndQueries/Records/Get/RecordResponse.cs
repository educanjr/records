
namespace BallastLane.Application.CommandAndQueries.Records.Get;

public sealed record RecordResponse(Guid Id, string Title, string Description, Guid CreatorId);
