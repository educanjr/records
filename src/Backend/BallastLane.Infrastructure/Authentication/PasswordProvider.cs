
using BallastLane.Application.Abstractions;

namespace BallastLane.Infrastructure.Authentication;

internal sealed class PasswordProvider : IPasswordProvider
{
    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool ComparePasswords(string password, string hashPassword) => BCrypt.Net.BCrypt.Verify(password, hashPassword);
}
