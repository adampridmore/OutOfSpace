using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkiaSharp;

namespace ViewImageWeb.Pages;

public class IndexModel: PageModel
{
    public IActionResult OnPost()
    {
        // Read the submitted image from the form
        var file = Request.Form.Files["file"];
        if (file == null)
        {
            return Page();
        }
        
        // Display it' size
        Filename = file.FileName;
        FileSize = file.Length;
        if (FileSize != 1024 * 1024 * 3)
            return Page();
        
        // Display the image
        using var stream = file.OpenReadStream();
        var bytes = new byte[file.Length];
        stream.ReadExactly(bytes, 0, (int)file.Length);
        
        // Draw to a canvas
        using var bmap = new SKBitmap(1024, 1024);

        for (var x = 0; x < 1024; x++)
        {
            for (var y = 0; y < 1024; y++)
            {
                var s = (1024 * 3 * y) + (x*3);
                bmap.SetPixel(x,y,new SKColor(bytes[s],bytes[s+1],bytes[s+2]));
            }
        }

        using var img = SKImage.FromBitmap(bmap);
        Image = img.Encode(SKEncodedImageFormat.Jpeg, 100).ToArray();

        Hash = ComputeHash(bytes);
        
        return Page();
    }

    public string ComputeHash(byte[] image)
    {
        var data = SHA256.HashData(image);
        var sBuilder = new StringBuilder();
        foreach (var t in data)
            sBuilder.Append(t.ToString("x2"));
        return sBuilder.ToString();
    }

    public byte[]? Image { get; set; }
    public long FileSize { get; set; }
    public string? Filename { get; set; }
    public string? Hash { get; set; }
}