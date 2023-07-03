
using BallastLane.Domain.Common;
using MediatR;

namespace BallastLane.Application.Abstractions;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, DomainResult<TResponse>> where TQuery : IQuery<TResponse>
{
}
