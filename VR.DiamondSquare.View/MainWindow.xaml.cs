using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace VR.DiamondSquare.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DrawHeightMapButton.IsEnabled = true;
            DrawNormalMapButton.IsEnabled = false;
            CleanButton.IsEnabled = false;
        }

        private void DrawHeightMapButton_Click(object sender, RoutedEventArgs e)
        {
            DrawHeightMapButton.IsEnabled = false;
            DrawNormalMapButton.IsEnabled = true;
            CleanButton.IsEnabled = true;
        }

        private void DrawNormalMapButton_Click(object sender, RoutedEventArgs e)
        {
            DrawHeightMapButton.IsEnabled = false;
            DrawNormalMapButton.IsEnabled = false;
            CleanButton.IsEnabled = true;
        }

        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            DrawHeightMapButton.IsEnabled = true;
            DrawNormalMapButton.IsEnabled = false;
            CleanButton.IsEnabled = false;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}