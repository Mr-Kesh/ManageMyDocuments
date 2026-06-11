namespace Document_Management_System.Models;

public class AttachmentRecord
{
    public int AttachmentId { get; set; }
    public string AttachmentName { get; set; } = string.Empty;
    public string AttachmentPath { get; set; } = string.Empty;
    public string AttachmentType { get; set; } = string.Empty;
    public int DocumentVersionId { get; set; }
}
