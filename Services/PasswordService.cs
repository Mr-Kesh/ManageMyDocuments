namespace Document_Management_System.Services;

public static class PasswordService
{
    public static string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public static bool Verify(string password, string storedHash)
    {
        // For testing seed data
        if (storedHash == "TEMP_PASSWORD_HASH")
        {
            return true;
        }

        if (storedHash.StartsWith("$2"))
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        return password == storedHash;
    }
}
