namespace Document_Management_System.Models;

public class VersionRecord
{
    public int VersionId { get; set; }
    public int VersionNumber { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime LastModifiedTime { get; set; }
    public int DocumentId { get; set; }
    public int LastModifiedBy { get; set; }
    public DateTime CreationTime { get; set; }
    public string LastModifiedByName { get; set; } = string.Empty;
}
