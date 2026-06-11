using Document_Management_System.Models;
using Document_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Document_Management_System.Pages.Versions;

public class DetailsModel : AuthenticatedPageModel
{
    private readonly AppDb _db;
    private readonly FileStorageService _files;

    public DetailsModel(AppDb db, FileStorageService files)
    {
        _db = db;
        _files = files;
    }

    public VersionRecord? Version { get; set; }
    public DocumentRecord? Document { get; set; }
    public List<AttachmentRecord> Attachments { get; set; } = [];
    public int AttachmentCount { get; set; }

    [BindProperty]
    public List<IFormFile>? UploadFiles { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var redirect = RequireLogin();
        if (redirect != null)
        {
            return redirect;
        }

        var loadResult = await LoadPageAsync(id);
        return loadResult ?? Page();
    }

    public async Task<IActionResult> OnPostUploadAsync(int id)
    {
        var redirect = RequireLogin();
        if (redirect != null)
        {
            return redirect;
        }

        var loadResult = await LoadPageAsync(id);
        if (loadResult != null)
        {
            return loadResult;
        }

        if (UploadFiles is null || UploadFiles.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "Choose at least one file.");
            return Page();
        }

        foreach (var file in UploadFiles)
        {
            var path = await _files.SaveUploadAsync(file, Version!.DocumentId, Version.VersionNumber);
            var contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;
            await _db.CreateAttachmentAsync(file.FileName, path, contentType, Version.VersionId);
        }

        TempData["Message"] = "Files uploaded.";
        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostDeleteAttachmentAsync(int id, int attachmentId)
    {
        var redirect = RequireLogin();
        if (redirect != null)
        {
            return redirect;
        }

        var loadResult = await LoadPageAsync(id);
        if (loadResult != null)
        {
            return loadResult;
        }

        var attachment = await _db.GetAttachmentByIdAsync(attachmentId);
        if (attachment is null || attachment.DocumentVersionId != id)
        {
            return NotFound();
        }

        _files.DeleteIfExists(attachment.AttachmentPath);
        await _db.DeleteAttachmentAsync(attachmentId);
        TempData["Message"] = "Attachment deleted.";
        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostDeleteVersionAsync(int id)
    {
        var redirect = RequireLogin();
        if (redirect != null)
        {
            return redirect;
        }

        var loadResult = await LoadPageAsync(id);
        if (loadResult != null)
        {
            return loadResult;
        }

        var documentId = Version!.DocumentId;
        foreach (var attachment in Attachments)
        {
            _files.DeleteIfExists(attachment.AttachmentPath);
        }

        await _db.DeleteVersionAsync(id);
        TempData["Message"] = "Version deleted.";
        return RedirectToPage("/Documents/Details", new { id = documentId });
    }

    private async Task<IActionResult?> LoadPageAsync(int versionId)
    {
        Version = await _db.GetVersionByIdAsync(versionId);
        if (Version is null)
        {
            return NotFound();
        }

        Document = await _db.GetDocumentByIdAsync(Version.DocumentId);
        if (Document is null)
        {
            return NotFound();
        }

        if (!IsAdmin && Document.CreatedBy != CurrentUserId)
        {
            return Forbid();
        }

        Attachments = await _db.ListAttachmentsByVersionAsync(versionId);
        AttachmentCount = await _db.CountAttachmentsByVersionAsync(versionId);
        return null;
    }
}
