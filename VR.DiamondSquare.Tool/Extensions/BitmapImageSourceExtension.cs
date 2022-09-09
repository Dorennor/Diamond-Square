using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VR.DiamondSquare.Tool.Extensions;

public static class BitmapImageSourceExtension
{
    public static ImageSource GetImageSource(this Bitmap bitmap)
    {
        var bitmapHandle = bitmap.GetHbitmap();

        return Imaging.CreateBitmapSourceFromHBitmap(bitmapHandle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
    }
}