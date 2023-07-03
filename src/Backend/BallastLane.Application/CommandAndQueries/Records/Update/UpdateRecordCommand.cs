
using BallastLane.Application.Abstractions;
using MediatR;

namespace BallastLane.Application.CommandAndQueries.Records.Update;

public sealed record UpdateRecordCommand(Guid Id, string? NewTitle, string? NewDescription, Guid CreatorId)
    : ICommand;
