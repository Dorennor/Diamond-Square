using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using VR.DiamondSquare.Tools.Extensions;
using VR.DiamondSquare.ViewModel.ViewModels;

namespace VR.DiamondSquare.ViewModel;

public partial class MainWindow : Window
{
    /// <summary>
    /// Regex that filtered only int strings.
    /// </summary>
    private Regex onlyIntRegex = new Regex(@"^[0-9]{0,10}$");

    public MainWindow()
    {
        InitializeComponent();
    }

    private void RangeTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        Match match = MainViewModel.FloatRangeRegex.Match(RangeTextBox.Text);

        if (!match.Success || !(Convert.ToSingle(match.Groups["max"].Value) > Convert.ToSingle(match.Groups["min"].Value)))
        {
            RangeTextBox.Text = string.Empty;

            MessageBox.Show("Wrong input, write range as \"min;max\".");
        }
    }

    private void SizeTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (SizeTextBox.Text != string.Empty)
        {
            var number = Convert.ToInt32(SizeTextBox.Text);

            if (number % 2 == 0 || !(number - 1).IsPowerOfTwo())
            {
                MessageBox.Show("Wrong input, write odd number that is a number that is a power of 2 and + 1.");

                SizeTextBox.Text = string.Empty;
            }
        }
        else
        {
            MessageBox.Show("Value can't be empty.");
        }
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !onlyIntRegex.IsMatch(e.Text);
    }
}