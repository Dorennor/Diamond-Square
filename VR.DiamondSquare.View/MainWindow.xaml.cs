using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using VR.DiamondSquare.Tools.Extensions;

namespace VR.DiamondSquare.ViewModel;

public partial class MainWindow : Window
{
    /// <summary>
    /// Regex that filtered only range strings, for example "1.2 ; 10.5" or "1, 19".
    /// </summary>
    private Regex floatRangeRegex = new Regex(@"^(?'min'[0-9]{1,6}(\.[0-9]{1,3})?)[ ]?\p{P}{1}[ ]?(?'max'[0-9]{1,6}(\.[0-9]{1,3})?)$");

    /// <summary>
    /// Regex that filtered only int strings.
    /// </summary>
    private Regex onlyIntRegex = new Regex("[^0-9]+");

    public MainWindow()
    {
        InitializeComponent();
    }

    private void RangeTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (!floatRangeRegex.IsMatch(RangeTextBox.Text))
        {
            RangeTextBox.Text = string.Empty;

            MessageBox.Show("Wrong input, write range as \"min;max\"");
        }
    }

    private void SizeTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var number = Convert.ToInt32(SizeTextBox.Text);

        if (number % 2 == 0)
        {
            MessageBox.Show("Wrong input, write odd number that is a number that is a power of 2 and + 1");
            SizeTextBox.Text = string.Empty;
            return;
        }

        number--;

        if (!number.IsPowerOfTwo())
        {
            SizeTextBox.Text = string.Empty;

            MessageBox.Show("Wrong input, write odd number that is a number that is a power of 2 and + 1");
        }
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        e.Handled = onlyIntRegex.IsMatch(e.Text);
    }
}