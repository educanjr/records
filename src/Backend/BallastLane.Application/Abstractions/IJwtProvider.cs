
using BallastLane.Domain.Entities;

namespace BallastLane.Application.Abstractions;

public interface IJwtProvider
{
    string Generate(User user);
}
