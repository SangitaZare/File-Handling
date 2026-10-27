using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace FileHandlingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QrController : ControllerBase
{
    [HttpGet]
    public IActionResult Generate([FromQuery] string name, [FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            return BadRequest("Name and email are required.");

        var text = $"Name: {name}\nEmail: {email}";

        using var generator = new QRCodeGenerator();
        using var data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var png = new PngByteQRCode(data);
        byte[] bytes = png.GetGraphic(20);

        return File(bytes, "image/png", "qrcode.png");
    }
}