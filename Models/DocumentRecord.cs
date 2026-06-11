namespace Document_Management_System.Models;

public class DocumentRecord
{
    public int DocumentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CrewName { get; set; } = string.Empty;
    public DateTime CreationTime { get; set; }
    public int CreatedBy { get; set; }
}
