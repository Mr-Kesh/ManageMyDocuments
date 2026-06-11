using Document_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Document_Management_System.Pages.Attachments;

public class DownloadModel : AuthenticatedPageModel
{
    // Service Fields
    private readonly AppDb _db;
    private readonly FileStorageService _files;

    public DownloadModel(AppDb db, FileStorageService files)
    {
        _db = db;
        _files = files;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var redirect = RequireLogin();
        if (redirect != null)
        {
            return redirect;
        }

        var attachment = await _db.GetAttachmentByIdAsync(id);
        if (attachment is null)
        {
            return NotFound();
        }

        var version = await _db.GetVersionByIdAsync(attachment.DocumentVersionId);
        if (version is null)
        {
            return NotFound();
        }

        var document = await _db.GetDocumentByIdAsync(version.DocumentId);
        if (document is null)
        {
            return NotFound();
        }

        if (!IsAdmin && document.CreatedBy != CurrentUserId)
        {
            return Forbid();
        }

        var absolutePath = _files.GetAbsolutePath(attachment.AttachmentPath);
        if (absolutePath is null || !System.IO.File.Exists(absolutePath))
        {
            return NotFound();
        }

        return PhysicalFile(absolutePath, attachment.AttachmentType, attachment.AttachmentName);
    }
}
