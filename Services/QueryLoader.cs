namespace Document_Management_System.Services;

public class QueryLoader
{
    private readonly string _queriesRoot;

    public QueryLoader(IWebHostEnvironment environment)
    {
        _queriesRoot = Path.Combine(environment.ContentRootPath, "SQL", "Queries");
    }

    public string Load(string folder, string fileName)
    {
        var path = Path.Combine(_queriesRoot, folder, fileName);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"SQL query not found: {path}");
        }

        return File.ReadAllText(path);
    }
}
