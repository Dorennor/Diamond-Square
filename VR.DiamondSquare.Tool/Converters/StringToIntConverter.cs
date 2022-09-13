using System.Globalization;
using System.Windows.Data;

namespace VR.DiamondSquare.Tools.Converters;

public class StringToIntConverter : IValueConverter
{
    public static IValueConverter Converter { get; private set; } = new StringToIntConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(value.ToString()))
        {
            return 0;
        }

        return System.Convert.ToInt32(value);
    }
}