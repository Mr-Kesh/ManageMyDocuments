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

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var redirect = RequireLogin();
        if (redirect != null)
        {
            return redirect;
        }

        Document = await _db.GetDocumentByIdAsync(id);
        if (Document is null)
        {
            return NotFound();
        }

        if (!IsAdmin && Document.CreatedBy != CurrentUserId)
        {
            return Forbid();
        }

        Versions = await _db.ListVersionsByDocumentAsync(id);
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var redirect = RequireLogin();
        if (redirect != null)
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
            return Forbid();
        }

        await _db.DeleteDocumentAsync(id);
        TempData["Message"] = "Document deleted.";
        return RedirectToPage("/Documents/Index");
    }
}
