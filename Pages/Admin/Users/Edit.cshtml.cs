using System.ComponentModel.DataAnnotations;
using Document_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Document_Management_System.Pages.Admin.Users;

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
        var redirect = RequireAdmin();
        if (redirect != null)
        {
            return redirect;
        }

        var user = await _db.GetUserByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        Input.UserId = user.UserId;
        Input.FullName = user.FullName;
        Input.Email = user.Email;
        Input.UserRole = user.UserRole;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var redirect = RequireAdmin();
        if (redirect != null)
        {
            return redirect;
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _db.AdminEditProfileAsync(Input.UserId, Input.Email, Input.UserRole, Input.FullName);
        TempData["Message"] = "User updated.";
        return RedirectToPage("/Admin/Users/Index");
    }

    public class InputModel
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Role")]
        public string UserRole { get; set; } = "User";
    }
}