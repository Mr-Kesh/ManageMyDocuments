using System.ComponentModel.DataAnnotations;
using Document_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Document_Management_System.Pages.Documents;

public class EditModel : AuthenticatedPageModel
{
    private readonly AppDb _db;

    public EditModel(AppDb db)
    {
        _db = db;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
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

        Input.DocumentId = document.DocumentId;
        Input.Title = document.Title;
        Input.CrewName = document.CrewName;
        return Page();
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

        var document = await _db.GetDocumentByIdAsync(Input.DocumentId);
        if (document is null)
        {
            return NotFound();
        }

        if (!IsAdmin && document.CreatedBy != CurrentUserId)
        {
            return Forbid();
        }

        await _db.UpdateDocumentAsync(Input.DocumentId, Input.Title, Input.CrewName);
        TempData["Message"] = "Document updated.";
        return RedirectToPage("/Documents/Details", new { id = Input.DocumentId });
    }

    public class InputModel
    {
        public int DocumentId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Crew name")]
        public string CrewName { get; set; } = string.Empty;
    }
}
