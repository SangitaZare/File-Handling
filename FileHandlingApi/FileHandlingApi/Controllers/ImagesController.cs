using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace FileHandlingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly string _folder;
    private static readonly string[] Allowed = { ".jpg", ".jpeg", ".png", ".gif" };

    public ImagesController(IWebHostEnvironment env)
    {
        _folder = Path.Combine(env.ContentRootPath, "Uploads");
        Directory.CreateDirectory(_folder);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file selected.");

        if (file.Length > 5 * 1024 * 1024)
            return BadRequest("File must be under 5 MB.");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!Allowed.Contains(ext))
            return BadRequest("Only jpg, jpeg, png and gif are allowed.");

        var newName = $"{Guid.NewGuid()}{ext}";
        var fullPath = Path.Combine(_folder, newName);

        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return Ok(new { fileName = newName });
    }

    [HttpGet("download/{fileName}")]
    public IActionResult Download(string fileName)
    {
        var safeName = Path.GetFileName(fileName);
        var fullPath = Path.Combine(_folder, safeName);

        if (!System.IO.File.Exists(fullPath))
            return NotFound();

        new FileExtensionContentTypeProvider()
            .TryGetContentType(safeName, out var contentType);

        return PhysicalFile(fullPath, contentType ?? "application/octet-stream", safeName);
    }
}