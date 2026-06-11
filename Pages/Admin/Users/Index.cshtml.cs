using Document_Management_System.Models;
using Document_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Document_Management_System.Pages.Admin.Users;

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
    public string? Role { get; set; }

    public List<UserRecord> Users { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        var redirect = RequireAdmin();
        if (redirect != null)
        {
            return redirect;
        }

        if (!string.IsNullOrWhiteSpace(Search))
        {
            Users = await _db.SearchUsersByNameAsync(Search);
        }
        else if (!string.IsNullOrWhiteSpace(Role))
        {
            Users = await _db.FilterUsersByRoleAsync(Role);
        }
        else
        {
            Users = await _db.ListUsersAsync();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int userId)
    {
        var redirect = RequireAdmin();
        if (redirect != null)
        {
            return redirect;
        }

        if (userId == CurrentUserId)
        {
            TempData["Message"] = "You cannot delete your own account while signed in.";
            return RedirectToPage();
        }

        await _db.DeleteUserAsync(userId);
        TempData["Message"] = "User deleted.";
        return RedirectToPage();
    }
}