namespace domain.Entities;
using static BCrypt.Net.BCrypt;

public class PasswordHasher
{
    private const int workFactor = 12;

    public static string GeneratePasswordHash(string password)
    {
        if(string.IsNullOrEmpty(password)) return "";

        var hashedPassword = HashPassword(password, workFactor);
        return hashedPassword;
    }

    public static bool VerifyPasswordHash(string password, string hash)
    {
        if(string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash)) return false;

        var passwordsMatch = Verify(password, hash);
        return passwordsMatch;
    }
}
