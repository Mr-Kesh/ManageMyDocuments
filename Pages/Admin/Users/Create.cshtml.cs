using System.ComponentModel.DataAnnotations;
using Document_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Document_Management_System.Pages.Admin.Users;

public class CreateModel : AuthenticatedPageModel
{
    private readonly AppDb _db;

    public CreateModel(AppDb db)
    {
        _db = db;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public IActionResult OnGet()
    {
        return RequireAdmin() ?? Page();
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

        var existing = await _db.GetUserByEmailAsync(Input.Email);
        if (existing is not null)
        {
            ModelState.AddModelError(nameof(Input.Email), "Email is already in use.");
            return Page();
        }

        await _db.CreateUserAsync(
            Input.Email,
            PasswordService.Hash(Input.Password),
            Input.UserRole,
            Input.FullName);

        TempData["Message"] = "User created.";
        return RedirectToPage("/Admin/Users/Index");
    }

    public class InputModel
    {
        [Required]
        [Display(Name = "Full name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Role")]
        public string UserRole { get; set; } = "User";
    }
}