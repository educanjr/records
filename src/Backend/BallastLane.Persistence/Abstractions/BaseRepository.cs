
using Dapper;

namespace BallastLane.Persistence.Abstractions;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
	private readonly IAppDbContext _context;

	protected IAppDbContext Context => _context;

    protected BaseRepository(IAppDbContext context)
	{
		_context = context;
	}

	protected async Task<IEnumerable<T>> ExecuteQueryAsync(string query, object? queryParams = null)
	{
		using var connection = _context.CreateConnection();
		connection.Open();
		return await connection.QueryAsync<T>(query, queryParams);
	}

	protected async Task<T> ExecuteQuerySingleOrDefaultAsync(string query, object? queryParams = null)
	{
		using var connection = _context.CreateConnection();
		return await connection.QuerySingleOrDefaultAsync<T>(query, queryParams);
	}

    protected async Task<int> ExecuteAsync(string query, object? queryParams = null)
    {
        using var connection = _context.CreateConnection();
        return await connection.ExecuteAsync(query, queryParams);
    }

    protected async Task<TResult> ExecuteScalarAsync<TResult>(string query, object? queryParams = null)
    {
        using var connection = _context.CreateConnection();
        return await connection.ExecuteScalarAsync<TResult>(query, queryParams);
    }
}
