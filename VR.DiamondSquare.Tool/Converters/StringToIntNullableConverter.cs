using System.Globalization;
using System.Windows.Data;

namespace VR.DiamondSquare.Tools.Converters;

public class StringToIntNullableConverter : IValueConverter
{
    public static IValueConverter Converter { get; private set; } = new StringToIntNullableConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return string.Empty;
        }

        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(value.ToString()))
        {
            return null;
        }

        return System.Convert.ToInt32(value);
    }
}