using System.ComponentModel.DataAnnotations;
using Document_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Document_Management_System.Pages.Documents;

public class CreateModel : AuthenticatedPageModel
{
    private readonly AppDb _db;
    private readonly FileStorageService _files;

    public CreateModel(AppDb db, FileStorageService files)
    {
        _db = db;
        _files = files;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public IActionResult OnGet()
    {
        return RequireLogin() ?? Page();
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
            return Page();
        }

        if (Input.Files is null || Input.Files.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "Upload at least one file.");
            return Page();
        }

        var documentId = await _db.CreateDocumentAsync(Input.Title, Input.CrewName, CurrentUserId!.Value);
        var versionNumber = await _db.GetNextVersionNumberAsync(documentId);
        var versionId = await _db.CreateVersionAsync(
            versionNumber,
            Input.ExpirationDate,
            documentId,
            CurrentUserId!.Value);

        foreach (var file in Input.Files)
        {
            var path = await _files.SaveUploadAsync(file, documentId, versionNumber);
            var contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;
            await _db.CreateAttachmentAsync(file.FileName, path, contentType, versionId);
        }

        TempData["Message"] = "Document created.";
        return RedirectToPage("/Documents/Details", new { id = documentId });
    }

    public class InputModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Crew name")]
        public string CrewName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Expiration date")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; } = DateTime.Today.AddYears(1);

        [Required]
        [Display(Name = "Files")]
        public List<IFormFile> Files { get; set; } = [];
    }
}
