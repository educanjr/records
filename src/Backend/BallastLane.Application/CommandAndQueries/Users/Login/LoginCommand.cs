
using BallastLane.Application.Abstractions;

namespace BallastLane.Application.CommandAndQueries.Users.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<string>;
