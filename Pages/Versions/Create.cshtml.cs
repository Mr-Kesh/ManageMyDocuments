using System.ComponentModel.DataAnnotations;
using Document_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Document_Management_System.Pages.Versions;

public class CreateModel : AuthenticatedPageModel
{
    private readonly AppDb _db;
    private readonly FileStorageService _files;

    public CreateModel(AppDb db, FileStorageService files)
    {
        _db = db;
        _files = files;
    }

    [BindProperty(SupportsGet = true)]
    public int DocumentId { get; set; }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string DocumentTitle { get; set; } = string.Empty;
    public int NextVersionNumber { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var redirect = RequireLogin();
        if (redirect != null)
        {
            return redirect;
        }

        return await LoadPageAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var redirect = RequireLogin();
        if (redirect != null)
        {
            return redirect;
        }

        if (!ModelState.IsValid)
        {
            await LoadPageAsync();
            return Page();
        }

        var document = await _db.GetDocumentByIdAsync(DocumentId);
        if (document is null)
        {
            return NotFound();
        }

        if (!IsAdmin && document.CreatedBy != CurrentUserId)
        {
            return Forbid();
        }

        if (Input.Files is null || Input.Files.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "Upload at least one file.");
            await LoadPageAsync();
            return Page();
        }

        var versionNumber = await _db.GetNextVersionNumberAsync(DocumentId);
        var versionId = await _db.CreateVersionAsync(
            versionNumber,
            Input.ExpirationDate,
            DocumentId,
            CurrentUserId!.Value);

        foreach (var file in Input.Files)
        {
            var path = await _files.SaveUploadAsync(file, DocumentId, versionNumber);
            var contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;
            await _db.CreateAttachmentAsync(file.FileName, path, contentType, versionId);
        }

        TempData["Message"] = $"Version {versionNumber} created.";
        return RedirectToPage("/Documents/Details", new { id = DocumentId });
    }

    private async Task<IActionResult> LoadPageAsync()
    {
        var document = await _db.GetDocumentByIdAsync(DocumentId);
        if (document is null)
        {
            return NotFound();
        }

        if (!IsAdmin && document.CreatedBy != CurrentUserId)
        {
            return Forbid();
        }

        DocumentTitle = document.Title;
        NextVersionNumber = await _db.GetNextVersionNumberAsync(DocumentId);
        Input.ExpirationDate = DateTime.Today.AddYears(1);
        return Page();
    }

    public class InputModel
    {
        [Required]
        [Display(Name = "Expiration date")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; } = DateTime.Today.AddYears(1);

        [Required]
        [Display(Name = "Files")]
        public List<IFormFile> Files { get; set; } = [];
    }
}
