
using Dapper;
using Npgsql;
using System.Data;

namespace BallastLane.Persistence;

public sealed class AppDbContext : IAppDbContext
{
	private readonly IDBSettings _dbSettings;
	public AppDbContext(IDBSettings dbSettings)
	{
		_dbSettings = dbSettings;
	}

	public IDbConnection CreateConnection() => new NpgsqlConnection(_dbSettings.ConnectionString);

	public async Task Init()
	{
		await InitDatabase();
		await InitTables();
	}

	private async Task InitDatabase()
	{
		// Create the DB if it don't exists
		using var connection = CreateConnection();
		
		var query = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{connection.Database}'";
		var dbCount = await connection.ExecuteScalarAsync<int>(query);
		if(dbCount <= 0)
		{
			var createQuery = $"CREATE DATABASE \"{connection.Database}\"";
			await connection.ExecuteAsync(createQuery);
		}
	}

	private async Task InitTables()
	{
		// create tables if they don't exist
		using var connection = CreateConnection();
		await initUsersTable();
		await initRecordsTable();

        async Task initUsersTable()
		{
			var sql = @"
                CREATE TABLE IF NOT EXISTS Users (
					Sec SERIAL PRIMARY KEY,
                    Id VARCHAR NOT NULL UNIQUE,
                    FirstName VARCHAR,
                    LastName VARCHAR,
                    Email VARCHAR NOT NULL UNIQUE,
                    PasswordHash VARCHAR NOT NULL
                );
            ";
            await connection.ExecuteAsync(sql);
		}

        async Task initRecordsTable()
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS Records (
					Sec SERIAL PRIMARY KEY,
                    Id VARCHAR NOT NULL UNIQUE,
                    Title VARCHAR,
                    Description VARCHAR,
                    CreatorId VARCHAR NOT NULL,
						CONSTRAINT fk_user_creator_id 
						FOREIGN KEY (CreatorId) REFERENCES Users(Id)
                );
            ";
            await connection.ExecuteAsync(sql);
        }
    }
}
