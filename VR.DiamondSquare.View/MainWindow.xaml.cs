using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace VR.DiamondSquare.ViewModel;

public partial class MainWindow : Window
{
    /// <summary>
    /// Regex that filtered only int strings.
    /// </summary>
    private static readonly Regex onlyIntRegex = new Regex(@"^[0-9]{0,9}$");

    private static readonly Regex moreThanZeroInt = new Regex(@"^[0-9]+$");

    public MainWindow()
    {
        InitializeComponent();
    }

    private void SeedNumberValidationWithEmptyStringTextBox(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !onlyIntRegex.IsMatch(e.Text);
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !moreThanZeroInt.IsMatch(e.Text);
    }
}