using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Diamond_Square.Extensions;
using Diamond_Square.Models;

namespace Diamond_Square
{
    public partial class MainWindow : Window
    {
        private static Random random;

        private CancellationTokenSource _CancellationTokenSource;
        private CancellationToken _CancellationToken;
        private Graphics _graphics;
        private Bitmap _bitmap;

        private float[,] heightmap;

        public MainWindow()
        {
            InitializeComponent();

            _CancellationTokenSource = new CancellationTokenSource();
            _CancellationToken = _CancellationTokenSource.Token;

            _bitmap = new Bitmap(1024, 1024);

            _graphics = Graphics.FromImage(_bitmap);
            _graphics.Clear(Color.Black);
            _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            DrawHeightMapButton.IsEnabled = true;
            DrawNormalMapButton.IsEnabled = false;
            StopButton.IsEnabled = false;
            CleanButton.IsEnabled = false;
        }

        private async void DrawHeightMapButton_Click(object sender, RoutedEventArgs e)
        {
            DrawHeightMapButton.IsEnabled = false;
            DrawNormalMapButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            CleanButton.IsEnabled = false;

            if (SeedTextBox.Text == String.Empty)
            {
                random = new Random();
            }
            else
            {
                random = new Random(Convert.ToInt32(SeedTextBox.Text));
            }

            await DrawHeightMapAsync(_CancellationToken);
        }

        private async void DrawNormalMapButton_Click(object sender, RoutedEventArgs e)
        {
            DrawHeightMapButton.IsEnabled = false;
            DrawNormalMapButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            CleanButton.IsEnabled = false;

            await DrawNormalMapAsync(_CancellationToken);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            DrawHeightMapButton.IsEnabled = true;
            DrawNormalMapButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            CleanButton.IsEnabled = true;

            _CancellationTokenSource.Cancel();
            _CancellationTokenSource.Dispose();

            _CancellationTokenSource = new CancellationTokenSource();
            _CancellationToken = _CancellationTokenSource.Token;
        }

        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            DrawHeightMapButton.IsEnabled = true;
            DrawNormalMapButton.IsEnabled = false;
            StopButton.IsEnabled = false;
            CleanButton.IsEnabled = false;

            _bitmap = new Bitmap(1024, 1024);

            _graphics = Graphics.FromImage(_bitmap);
            _graphics.Clear(Color.Black);
            _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        private async Task DrawHeightMapAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            heightmap = HeightMapping.Generate(random);

            await Task.Run(() =>
            {
                for (int i = 0; i < HeightMapping.XSize; i++)
                {
                    for (int j = 0; j < HeightMapping.YSize; j++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        int opacity = (int)Math.Round(heightmap[i, j] * 255);

                        if (opacity < 0)
                        {
                            opacity = 0;
                        }
                        else if (opacity > 255)
                        {
                            opacity = 255;
                        }

                        var pen = new Pen(Color.FromArgb(opacity, 255, 255, 255), 1);

                        lock (_graphics)
                        {
                            _graphics.DrawRectangle(pen, i, j, 1, 1);
                        }

                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            lock (_bitmap)
                            {
                                FractalImage.Source = _bitmap.GetImageSource();
                            }
                        }), System.Windows.Threading.DispatcherPriority.Background);
                    }
                }
            });

            DrawHeightMapButton.IsEnabled = false;
            DrawNormalMapButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            CleanButton.IsEnabled = true;
        }

        private async Task DrawNormalMapAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            await Task.Run(new Action(() =>
            {
                lock (_bitmap)
                {
                    _bitmap = NormalMapping.Calculate(_bitmap);
                }
            }));

            DrawHeightMapButton.IsEnabled = false;
            DrawNormalMapButton.IsEnabled = false;
            StopButton.IsEnabled = false;
            CleanButton.IsEnabled = true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}