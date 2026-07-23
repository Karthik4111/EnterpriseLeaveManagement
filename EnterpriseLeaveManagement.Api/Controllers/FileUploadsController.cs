using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.FileUploads.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseLeaveManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FileUploadsController : ControllerBase
{
    private readonly IFileStorageService _fileStorageService;

    public FileUploadsController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("File Upload API is working.");
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(
        IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Please select a file.");
        }

        const long maxFileSize = 5 * 1024 * 1024; // 5 MB

        if (file.Length > maxFileSize)
        {
            return BadRequest("File size cannot exceed 5 MB.");
        }

        var allowedExtensions = new[]
        {
            ".pdf",
            ".jpg",
            ".jpeg",
            ".png"
        };

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest("Only PDF, JPG, JPEG and PNG files are allowed.");
        }

        await using var stream = file.OpenReadStream();

        var filePath = await _fileStorageService.SaveFileAsync(
            stream,
            file.FileName,
            file.ContentType,
            cancellationToken);

        var response = new FileUploadDto
        {
            FileName = file.FileName,
            FilePath = filePath,
            FileSize = file.Length,
            ContentType = file.ContentType
        };

        return Ok(response);
    }
}