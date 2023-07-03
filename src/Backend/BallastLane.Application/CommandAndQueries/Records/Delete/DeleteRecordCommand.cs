
using BallastLane.Application.Abstractions;
using MediatR;

namespace BallastLane.Application.CommandAndQueries.Records.Delete;

public sealed record DeleteRecordCommand(Guid Id, Guid CreatorId)
    : ICommand;
