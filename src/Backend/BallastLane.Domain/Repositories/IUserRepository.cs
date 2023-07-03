
using BallastLane.Domain.Entities;

namespace BallastLane.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);

    Task Add(User user);
    Task Update(User user);
}
