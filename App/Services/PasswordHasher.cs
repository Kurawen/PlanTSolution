using BCrypt.Net;

namespace App.Services;
public class PasswordHasher : IPasswordHasher
{
    public string GenerateHashPassword(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    public bool VerifyPassword(string password, string hashedPassword) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
}
