using System.ComponentModel.DataAnnotations;
using Document_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Document_Management_System.Pages.Versions;

public class EditModel : AuthenticatedPageModel
{
    private readonly AppDb _db;

    public EditModel(AppDb db)
    {
        _db = db;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string DocumentTitle { get; set; } = string.Empty;
    public int VersionNumber { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        return RequireLogin() ?? await LoadPageAsync(id);

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
            await LoadPageAsync(Input.VersionId);
            return Page();
        }

        var version = await _db.GetVersionByIdAsync(Input.VersionId);
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

        await _db.UpdateVersionAsync(Input.VersionId, Input.ExpirationDate, CurrentUserId!.Value);
        TempData["Message"] = "Version Updated Successfully!";
        return RedirectToPage("/Versions/Details", new { id = Input.VersionId });
    }

    private async Task<IActionResult> LoadPageAsync(int versionId)
    {
        var version = await _db.GetVersionByIdAsync(versionId);
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

        Input.VersionId = versionId;
        Input.ExpirationDate = version.ExpirationDate.Date;
        DocumentTitle = document.Title;
        VersionNumber = version.VersionNumber;
        return Page();
    }

    public class InputModel
    {
        public int VersionId { get; set; }

        [Required]
        [Display(Name = "Expiration Date")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }
    }
}