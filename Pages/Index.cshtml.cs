using Document_Management_System.Constants;
using Document_Management_System.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Document_Management_System.Pages;

public class IndexModel : PageModel
{
    private readonly AppDb _db;

    public IndexModel(AppDb db)
    {
        _db = db;
    }

    public string? WelcomeName { get; set; }
    public int DocumentCount { get; set; }
    public int ExpiringCount { get; set; }
    public bool IsSignedIn { get; set; }
    public bool IsAdmin { get; set; }

    public async Task OnGetAsync()
    {
        var userId = HttpContext.Session.GetInt32(SessionKeys.UserId);
        if (userId is null)
        {
            return;
        }

        IsSignedIn = true;
        WelcomeName = HttpContext.Session.GetString(SessionKeys.UserFullName);
        IsAdmin = string.Equals(HttpContext.Session.GetString(SessionKeys.UserRole), "Admin", StringComparison.OrdinalIgnoreCase);

        var documents = IsAdmin
            ? await _db.ListAllDocumentsAsync()
            : await _db.ListDocumentsByUserAsync(userId.Value);

        DocumentCount = documents.Count;
        ExpiringCount = (await _db.ListVersionsExpiringSoonAsync(3)).Count;
    }
}