using Document_Management_System.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Document_Management_System.Pages;

public abstract class AuthenticatedPageModel : PageModel
{
    // Helper Properties
    protected int? CurrentUserId => HttpContext.Session.GetInt32(SessionKeys.UserId);

    protected string? CurrentUserRole => HttpContext.Session.GetString(SessionKeys.UserRole);

    protected bool IsAdmin => string.Equals(CurrentUserRole, "Admin", StringComparison.OrdinalIgnoreCase);

    protected IActionResult? RequireLogin()
    {
        return CurrentUserId is null ? RedirectToPage("/Account/SignIn") : null;
    }

    protected IActionResult? RequireAdmin()
    {
        var loginResult = RequireLogin();
        if (loginResult != null)
        {
            return loginResult;
        }

        return IsAdmin ? null : RedirectToPage("/Index");
    }
}