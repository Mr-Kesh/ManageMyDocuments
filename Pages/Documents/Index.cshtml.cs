using Document_Management_System.Models;
using Document_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Document_Management_System.Pages.Documents;

public class IndexModel : AuthenticatedPageModel
{
    private readonly AppDb _db;

    public IndexModel(AppDb db)
    {
        _db = db;
    }

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Crew { get; set; }

    public List<DocumentRecord> Documents { get; set; } = [];

    public bool IsAdminUser => IsAdmin;
    public int? CurrentUserIdValue => CurrentUserId;

    public async Task<IActionResult> OnGetAsync()
    {
        var redirect = RequireLogin();
        if (redirect != null)
        {
            return redirect;
        }

        if (!string.IsNullOrWhiteSpace(Search) && !string.IsNullOrWhiteSpace(Crew))
        {
            Documents = await _db.SearchDocumentsByTitleAndCrewAsync(Search, Crew);
        }
        else if (!string.IsNullOrWhiteSpace(Search))
        {
            Documents = await _db.SearchDocumentsByTitleAsync(Search);
        }
        else if (!string.IsNullOrWhiteSpace(Crew)) {
            Documents = await _db.FilterDocumentsByCrewAsync(Crew);
        }
        else
        {
            Documents = await _db.ListAllDocumentsAsync();
        }

        return Page();
    }


    public async Task<IActionResult> OnPostDeleteAsync(int documentId)
    {
        var redirect = RequireLogin();
        if (redirect is not null)
        {
            return redirect;
        }

        var document = await _db.GetDocumentByIdAsync(documentId);
        if (document is null)
        {
            return NotFound();
        }

        if (!IsAdmin && document.CreatedBy != CurrentUserId)
        {
            return StatusCode(403);
        }

        await _db.DeleteDocumentAsync(documentId);

        TempData["Message"] = "Document Successfully Deleted!";
        return RedirectToPage();
    }
}