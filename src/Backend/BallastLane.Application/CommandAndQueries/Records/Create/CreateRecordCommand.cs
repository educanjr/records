
using BallastLane.Application.Abstractions;
using MediatR;

namespace BallastLane.Application.CommandAndQueries.Records.Create;

public sealed record CreateRecordCommand(Guid CreatorId, string Title, string Description)
    : ICommand<Guid>;
