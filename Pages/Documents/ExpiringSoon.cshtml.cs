using Document_Management_System.Models;
using Document_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Document_Management_System.Pages.Documents;

public class ExpiringSoonModel : AuthenticatedPageModel
{
    private readonly AppDb _db;

    public ExpiringSoonModel(AppDb db)
    {
        _db = db;
    }

    [BindProperty(SupportsGet = true)]
    public int Months { get; set; } = 3;

    public List<ExpiringVersionRecord> Versions { get; set; } = [];


    public async Task<IActionResult> OnGetAsync()
    {
        if (Months < 1)
        {
            Months = 3;
        }
        else if (Months > 24)
        {
            Months = 24;
        }

        return RequireLogin() ?? await LoadPageDataAsync();
    }


    public async Task<IActionResult> LoadPageDataAsync()
    {
        Versions = await _db.ListVersionsExpiringSoonAsync(Months);
        return Page();
    }
}