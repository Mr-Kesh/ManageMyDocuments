using System.ComponentModel.DataAnnotations;
using Document_Management_System.Constants;
using Document_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Document_Management_System.Pages.Account;

public class ChangePasswordModel : AuthenticatedPageModel
{
    private readonly AppDb _db;

    public ChangePasswordModel(AppDb db)
    {
        _db = db;
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

        var user = await _db.GetUserByEmailAsync(HttpContext.Session.GetString(SessionKeys.UserEmail) ?? string.Empty);
        if (user is null || !PasswordService.Verify(Input.CurrentPassword, user.PasswordHash))
        {
            ModelState.AddModelError("Input.CurrentPassword", "Current password is incorrect.");
            return Page();
        }

        await _db.ChangePasswordAsync(CurrentUserId!.Value, PasswordService.Hash(Input.NewPassword));
        TempData["Message"] = "Password changed.";
        return RedirectToPage("/Account/Profile");
    }

    public class InputModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(NewPassword))]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}