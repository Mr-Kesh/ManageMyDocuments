namespace Document_Management_System.Services;

public class FileStorageService
{
    private readonly IWebHostEnvironment _environment;

    public FileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveUploadAsync(IFormFile file, int documentId, int versionId)
    {
        var fileName = Path.GetFileName(file.FileName);
        var relativeDirectory = Path.Combine("uploads", documentId.ToString(), $"V{versionId}");
        var absoluteDirectory = Path.Combine(_environment.WebRootPath, relativeDirectory);
        Directory.CreateDirectory(absoluteDirectory);

        var absolutePath = Path.Combine(absoluteDirectory, fileName);
        await using var createFile = File.Create(absolutePath);
        await file.CopyToAsync(createFile);

        return "/" + relativeDirectory.Replace('\\', '/') + '/' + fileName;
    }

    public string? GetAbsolutePath(string webRelativePath)
    {
        if (string.IsNullOrWhiteSpace(webRelativePath))
        {
            return null;
        }

        var trimmed = webRelativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        return Path.Combine(_environment.WebRootPath, trimmed);
    }

    public void DeleteIfExists(string? webRelativePath)
    {
        var absolutePath = GetAbsolutePath(webRelativePath ?? string.Empty);
        if (absolutePath != null && File.Exists(absolutePath))
        {
            File.Delete(absolutePath);
        }
    }
}
