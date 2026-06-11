using System.ComponentModel.DataAnnotations;
using Document_Management_System.Constants;
using Document_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Document_Management_System.Pages.Account;

public class ProfileModel : AuthenticatedPageModel
{
    private readonly AppDb _db;

    public ProfileModel(AppDb db)
    {
        _db = db;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var redirect = RequireLogin();
        if (redirect != null)
        {
            return redirect;
        }

        var user = await _db.GetUserByIdAsync(CurrentUserId!.Value);
        if (user is null)
        {
            return RedirectToPage("/Account/SignOut");
        }

        Input.Email = user.Email;
        Input.FullName = user.FullName;
        Input.Role = user.UserRole;
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

        await _db.SelfEditProfileAsync(CurrentUserId!.Value, Input.Email, Input.FullName);
        HttpContext.Session.SetString(SessionKeys.UserEmail, Input.Email);
        HttpContext.Session.SetString(SessionKeys.UserFullName, Input.FullName);

        TempData["Message"] = "Profile Updated Successfully!";
        return RedirectToPage();
    }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Full name")]
        public string FullName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}