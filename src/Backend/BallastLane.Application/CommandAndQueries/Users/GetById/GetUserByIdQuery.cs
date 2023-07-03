
using BallastLane.Application.Abstractions;

namespace BallastLane.Application.CommandAndQueries.Users.GetById;

public sealed record GetUserByIdQuery(Guid Id) : IQuery<GetUserByIdResponse>;
