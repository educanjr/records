
namespace BallastLane.Presentation.Contracts;

public sealed record RegisterRecordRequest(
        string Title,
        string Description,
        Guid CreatorId);
