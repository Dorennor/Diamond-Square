using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using VR.DiamondSquare.Tool.Extensions;

namespace VR.DiamondSquare.ViewModel.Converters
{
    public class BitmapSourceConverter : IValueConverter
    {
        public static IValueConverter Converter { get; private set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bitmap = (Bitmap)value;

            if (bitmap == null)
            {
                return null;
            }

            return bitmap.GetImageSource();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}