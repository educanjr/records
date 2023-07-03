using BallastLane.Domain.Common;
using MediatR;

namespace BallastLane.Application.Abstractions;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, DomainResult>
    where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse>  : IRequestHandler<TCommand, DomainResult<TResponse>>
    where TCommand : ICommand<TResponse>
{
}
