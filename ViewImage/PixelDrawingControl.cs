using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using SkiaSharp;

namespace ViewImage;

public class PixelDrawingControl : Control
{
    private new const int Width = 1024;
    private new const int Height = 1024;
    private readonly WriteableBitmap _bitmap = new(new PixelSize(Width/2, Height), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Unpremul);


    public string BaseFolder { get; set; } = "";
    
    public byte[]? DisplayFile(string filename)
    {
        var file = File.ReadAllBytes(Path.Combine(BaseFolder, filename));
        if (file.Length != 1024 * 1024 * 3)
        {
            ClearImage();
            return null;
        }

        var x = 0;
        var y = 0;
        for (var i = 0; i < file.Length; i+=6)
        {
            var r = file[i];
            var g = file[i+1];
            var b = file[i+2];
            DrawPixel(x,y, r,g,b,255);
            x++;

            if (x >= Width)
            {
                y++;
                x = 0;
            }
        }
        this.InvalidateVisual();

        return file;
    }

    private void ClearImage()
    {
        for (var x = 0; x < Width/2; x++)
        {
            for (var y = 0; y < Height/2; y++)
            {
                DrawPixel(x,y,0,0,0,255);
            }
        }
    }

    private void DrawPixel(int x, int y, byte r, byte g, byte b, byte a)
    {
        using var fb = _bitmap.Lock();
        unsafe
        {
            var ptr = (uint*)fb.Address;
            var index = y * fb.RowBytes / 4 + x;
            ptr[index] = (uint)(a << 24 | r << 16 | g << 8 | b);
        }
    }

    public Bitmap SKBitmapToAvaloniaBitmap(SKBitmap skBitmap)
    {
        var data = skBitmap.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = data.AsStream();
        return new Bitmap(stream);
    }
    
    public override void Render(DrawingContext context)
    {
        context.DrawImage(_bitmap, new Rect(0, 0, _bitmap.Size.Width, _bitmap.Size.Height));
    }
}