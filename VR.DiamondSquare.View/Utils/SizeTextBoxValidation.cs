using System;
using System.Globalization;
using System.Windows.Controls;
using VR.DiamondSquare.Tools.Extensions;

namespace VR.DiamondSquare.View.Utils;

public class SizeTextBoxValidation : ValidationRule
{
    public SizeTextBoxValidation()
    {
    }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (!string.IsNullOrEmpty((string)value))
        {
            var number = Convert.ToInt32((string)value);

            if (number % 2 == 0 || !(number - 1).IsPowerOfTwo())
            {
                return new ValidationResult(false, "Wrong input, write odd number that is a number that is a power of 2 and + 1.");
            }
        }
        else
        {
            return new ValidationResult(false, "Value can't be empty.");
        }

        return ValidationResult.ValidResult;
    }
}