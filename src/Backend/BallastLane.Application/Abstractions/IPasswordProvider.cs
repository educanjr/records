
namespace BallastLane.Application.Abstractions;

public interface IPasswordProvider
{
    string HashPassword(string password);

    bool ComparePasswords(string password, string hashPassword);
}
