
using System.Data;

namespace BallastLane.Persistence;

public interface IAppDbContext
{
    IDbConnection CreateConnection();

    Task Init();
}
