namespace Document_Management_System.Models;

public class UserRecord
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
}
