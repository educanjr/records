
using BallastLane.Application.Abstractions;

namespace BallastLane.Application.CommandAndQueries.Users.Create;

public sealed record CreateUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password) : ICommand<Guid>;
