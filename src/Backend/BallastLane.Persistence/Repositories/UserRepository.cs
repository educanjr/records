
using BallastLane.Domain.Entities;
using BallastLane.Domain.Repositories;
using BallastLane.Persistence.Abstractions;
using BallastLane.Persistence.DTOs;
using AutoMapper;

namespace BallastLane.Persistence.Repositories;

public class UserRepository : BaseRepository<UserDTO>, IUserRepository
{
    private readonly IMapper _mapper;
    public UserRepository(IAppDbContext context, IMapper mapper)
        : base(context)
    {
        _mapper = mapper;
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var query = """
            SELECT * FROM Users 
            WHERE Email = @email
        """;

        var userDTO = await ExecuteQuerySingleOrDefaultAsync(query, new { email });

        return _mapper.Map<User>(userDTO);
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = """
            SELECT * FROM Users 
            WHERE Id = @id
        """;

        var userDTO = await ExecuteQuerySingleOrDefaultAsync(query, new { id = id.ToString() });
        return _mapper.Map<User>(userDTO);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        var query = """
            SELECT COUNT(Email) FROM Users 
            WHERE Email = @email
        """;

        var emailCount = await ExecuteScalarAsync<int>(query, new { email });

        return emailCount <= 0;
    }

    public async Task Add(User user)
    {
        var userDTO = _mapper.Map<UserDTO>(user);
        var query = """
            INSERT INTO Users (Id, FirstName, LastName, Email, PasswordHash)
            VALUES (@Id, @FirstName, @LastName, @Email, @PasswordHash)
        """;

        await ExecuteAsync(query, userDTO);
    }

    public async Task Update(User user)
    {
        var userDTO = _mapper.Map<UserDTO>(user);
        var query = """
            UPDATE Users 
            SET FirstName = @FirstName,
                LastName = @LastName
            WHERE Id = @Id
        """;

        await ExecuteAsync(query, userDTO);
    }
}
