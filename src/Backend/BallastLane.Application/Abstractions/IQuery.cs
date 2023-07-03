
using BallastLane.Domain.Common;
using MediatR;

namespace BallastLane.Application.Abstractions;

public interface IQuery<TResponse> : IRequest<DomainResult<TResponse>>
{
}
