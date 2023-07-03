using BallastLane.Domain.Common;
using MediatR;

namespace BallastLane.Application.Abstractions;

public interface ICommand : IRequest<DomainResult>
{
}

public interface ICommand<TResponse> : IRequest<DomainResult<TResponse>>
{
}