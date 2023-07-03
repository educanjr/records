
using System.Data.Common;

namespace BallastLane.Persistence;

public sealed class DBSettings : IDBSettings
{
	private readonly string? _connectionString;
	public string ConnectionString => _connectionString ?? throw new ApplicationException("Connection string was not provided");

	public DBSettings()
	{
		_connectionString = string.Empty;
	}

	public DBSettings(string? connectionString)
	{
		_connectionString = connectionString;
	}
}
