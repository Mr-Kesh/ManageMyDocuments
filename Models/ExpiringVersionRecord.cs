namespace Document_Management_System.Models;

public class ExpiringVersionRecord
{
    public int VersionId { get; set; }
    public int VersionNumber { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int DocumentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CrewName { get; set; } = string.Empty;
}
