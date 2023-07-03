
using BallastLane.Application.Abstractions;

namespace BallastLane.Application.CommandAndQueries.Users.Update;

public sealed record UpdateUserCommand(Guid id, string FirstName, string LastName) : ICommand;
