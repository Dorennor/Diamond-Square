using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace VR.DiamondSquare.Tool.Extensions
{
    public static class BitmapImageSourceExtension
    {
        public static BitmapImage GetImageSource(this Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;

                BitmapImage bitmapimage = new BitmapImage();

                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        //[DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool DeleteObject([In] IntPtr hObject);

        //public static ImageSource GetImageSource(this Bitmap bmp)
        //{
        //    var handle = bmp.GetHbitmap();
        //    try
        //    {
        //        return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        //    }
        //    finally { DeleteObject(handle); }
        //}
    }
}