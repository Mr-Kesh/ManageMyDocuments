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

    public DocumentRecord Document { get; set; } = null!;
    public VersionRecord Version { get; set; } = null!;
    public List<AttachmentRecord> Attachments { get; set; } = [];
    public int? AttachmentCount { get; set; } = 0;

    public bool IsAdminUser => IsAdmin;
    public int? CurrentUserIdValue => CurrentUserId;

    [BindProperty]
    public List<IFormFile>? UploadFiles { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        return RequireLogin() ?? await Validation(id) ?? await LoadPageDataAsync(id);
    }


    public async Task<IActionResult> OnPostDeleteVersionAsync(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null)
        {
            return redirect;
        }

        var validation = await Validation(id);
        if (validation is not null)
        {
            return validation;
        }

        if (!IsAdmin && Document.CreatedBy != CurrentUserId)
        {
            return StatusCode(403);
        }

        var attachments = await _db.ListAttachmentsByVersionAsync(id);
        foreach (var attachment in attachments)
        {
            _files.DeleteIfExists(attachment.AttachmentPath);
        }

        await _db.DeleteVersionAsync(id);

        TempData["Message"] = "Version Deleted Successfully!";

        var documentId = Version!.DocumentId;
        return RedirectToPage("/Documents/Details", new { id = documentId });
    }


    public async Task<IActionResult> OnPostDeleteAttachmentAsync(int id, int attachmentId)
    {
        var redirect = RequireLogin();
        if (redirect is not null)
        {
            return redirect;
        }

        var validation = await Validation(id);
        if (validation is not null)
        {
            return validation;
        }

        if (!IsAdmin && Document.CreatedBy != CurrentUserId)
        {
            return StatusCode(403);
        }

        var attachment = await _db.GetAttachmentByIdAsync(attachmentId);
        if (attachment is null || (attachment.DocumentVersionId != id))
        {
            return NotFound();
        }

        _files.DeleteIfExists(attachment.AttachmentPath);
        await _db.DeleteAttachmentAsync(attachmentId);
        TempData["Message"] = "Attachment Deleted Successfully!";
        return RedirectToPage(new { id });
    }


    public async Task<IActionResult> OnPostUploadAsync(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null)
        {
            return redirect;
        }

        var validation = await Validation(id);
        if (validation is not null)
        {
            return validation;
        }

        if (!IsAdmin && Document.CreatedBy != CurrentUserId)
        {
            return StatusCode(403);
        }

        if (UploadFiles is null || UploadFiles.Count == 0)
        {
            ModelState.AddModelError("UploadFiles", "Upload At Least One File!");
            return await LoadPageDataAsync(id);
        }

        foreach (var file in UploadFiles)
        {
            var path = await _files.SaveUploadAsync(file, Document.DocumentId, Version.VersionNumber);
            var contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;
            await _db.CreateAttachmentAsync(file.FileName, path, contentType, id);
        }

        TempData["Message"] = "Attachment Uploaded Successfully!";
        return RedirectToPage(new { id });
    }


    public async Task<IActionResult> LoadPageDataAsync(int id)
    {
        Version = (await _db.GetVersionByIdAsync(id))!;
        Document = (await _db.GetDocumentByIdAsync(Version.DocumentId))!;
        Attachments = await _db.ListAttachmentsByVersionAsync(id);
        AttachmentCount = await _db.CountAttachmentsByVersionAsync(id);
        return Page();
    }


    private async Task<IActionResult?> Validation(int id)
    {
        var verison = await _db.GetVersionByIdAsync(id);
        if (verison is null)
        {
            return NotFound();
        }

        var document = await _db.GetDocumentByIdAsync(verison.DocumentId);
        if (document is null)
        {
            return NotFound();
        }

        Version = verison;
        Document = document;
        return null;
    }
}