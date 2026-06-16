using Document_Management_System.Models;
using Document_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Document_Management_System.Pages.Documents;

public class DetailsModel : AuthenticatedPageModel
{
    private readonly AppDb _db;

    public DetailsModel(AppDb db)
    {
        _db = db;
    }

    public DocumentRecord? Document { get; set; }
    public List<VersionRecord> Versions { get; set; } = [];

    public bool IsAdminUser => IsAdmin;
    public int? CurrentUserIdValue => CurrentUserId;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        return RequireLogin() ?? await LoadPageDataAsync(id);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null)
        {
            return redirect;
        }

        var document = await _db.GetDocumentByIdAsync(id);
        if (document is null)
        {
            return NotFound();
        }

        if (!IsAdmin && document.CreatedBy != CurrentUserId)
        {
            return StatusCode(403);
        }

        await _db.DeleteDocumentAsync(id);
        TempData["Message"] = "Document Deleted Successfully!";
        return RedirectToPage("/Documents/Index");
    }

    private async Task<IActionResult> LoadPageDataAsync(int id)
    {
        Document = await _db.GetDocumentByIdAsync(id);
        if (Document is null)
        {
            return NotFound();
        }

        Versions = await _db.ListVersionsByDocumentAsync(id);
        foreach (var version in Versions)
        {
            version.LastModifiedByName = await _db.GetLastModifiedByNameAsync(version.VersionId) ?? "Unknown";
        }

        return Page();
    }
}
