using System.ComponentModel.DataAnnotations;
using Document_Management_System.Constants;
using Document_Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Document_Management_System.Pages.Account;

public class SignInModel : PageModel
{
    private readonly AppDb _db;

    public SignInModel(AppDb db)
    {
        _db = db;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetInt32(SessionKeys.UserId) is not null)
        {
            return RedirectToPage("/Index");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _db.GetUserByEmailAsync(Input.Email);
        if (user is null || !PasswordService.Verify(Input.Password, user.PasswordHash))
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return Page();
        }

        var profile = await _db.GetUserByIdAsync(user.UserId);

        HttpContext.Session.SetInt32(SessionKeys.UserId, profile!.UserId);
        HttpContext.Session.SetString(SessionKeys.UserEmail, profile.Email);
        HttpContext.Session.SetString(SessionKeys.UserRole, profile.UserRole);
        HttpContext.Session.SetString(SessionKeys.UserFullName, profile.FullName);

        return RedirectToPage("/Index");
    }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
    }
}