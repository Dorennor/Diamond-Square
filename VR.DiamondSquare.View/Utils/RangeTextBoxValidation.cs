using System.Globalization;
using System.Windows.Controls;
using VR.DiamondSquare.ViewModel.ViewModels;

namespace VR.DiamondSquare.View.Utils;

public class RangeTextBoxValidation : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        string str = (string)value;
        if (str != null && str != string.Empty)
        {
            if(!MainViewModel.FloatRangeRegex.IsMatch(str))
            {
                return new ValidationResult(false, "Wrong input, write range as \"min;max\".");
            }
        }
        else
        {
            return new ValidationResult(false, "Value can't be empty");
        }

        return ValidationResult.ValidResult;
    }
}