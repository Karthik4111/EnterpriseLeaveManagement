using EnterpriseLeaveManagement.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace EnterpriseLeaveManagement.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;

    public FileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveFileAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken)
    {
        var webRootPath = _environment.WebRootPath;

        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        }

        var uploadsFolder = Path.Combine(webRootPath, "uploads");

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";

        var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

        await using var outputStream = new FileStream(
            fullPath,
            FileMode.Create);

        await fileStream.CopyToAsync(outputStream, cancellationToken);

        return $"/uploads/{uniqueFileName}";
    }

    public Task DeleteFileAsync(string filePath)
    {
        var fullPath = Path.Combine(
            _environment.WebRootPath,
            filePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }
}