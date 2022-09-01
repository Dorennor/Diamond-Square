using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Diamond_Square.Extensions;
using Diamond_Square.Interfaces;
using Diamond_Square.Models;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;

namespace Diamond_Square
{
    public partial class MainWindow : Window
    {
#pragma warning disable CS8618
        private static IRandomGenerator _randomGenerator;
#pragma warning restore CS8618

        private INormalMapping _normalMapping;
        private IHeightMapping _heightMapping;

        private CancellationTokenSource _CancellationTokenSource;
        private CancellationToken _CancellationToken;
        private Graphics _graphics;
        private Bitmap _bitmap;
        private static Pen[] pens;
        private int[,] heightMap;

        static MainWindow()
        {
            pens = new Pen[256];

            for (int i = 0; i < pens.Length; i++)
            {
                pens[i] = new Pen(Color.FromArgb(255, i, i, i), 1);
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            _normalMapping = new NormalMapping();
            _heightMapping = new HeightMapping();

            _CancellationTokenSource = new CancellationTokenSource();
            _CancellationToken = _CancellationTokenSource.Token;

            _bitmap = new Bitmap(HeightMapping.XSize, HeightMapping.YSize);

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
                _randomGenerator = new RandomGenerator();
            }
            else
            {
                _randomGenerator = new RandomGenerator(Convert.ToInt32(SeedTextBox.Text));
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

            _bitmap = new Bitmap(HeightMapping.XSize, HeightMapping.YSize);

            _graphics = Graphics.FromImage(_bitmap);
            _graphics.Clear(Color.White);
            _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            FractalImage.Source = _bitmap.GetImageSource();
        }

        private async Task DrawHeightMapAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            heightMap = _heightMapping.GenerateHeightMap(_randomGenerator);

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

                        _graphics.DrawRectangle(pens[heightMap[i, j]], i, j, 1, 1);
                    }
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    FractalImage.Source = _bitmap.GetImageSource();
                }), System.Windows.Threading.DispatcherPriority.Background);
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
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _bitmap = _normalMapping.GenerateNormalMap(_bitmap);
                    FractalImage.Source = _bitmap.GetImageSource();
                }), System.Windows.Threading.DispatcherPriority.Background);
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