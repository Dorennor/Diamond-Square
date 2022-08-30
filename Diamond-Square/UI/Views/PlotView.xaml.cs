using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Diamond_Square.Extensions;
using Color = System.Drawing.Color;

namespace Diamond_Square.UI.Views
{
    public partial class PlotView : UserControl
    {
        private static Random random = new Random(1);

        private CancellationTokenSource CancellationTokenSource;
        private CancellationToken CancellationToken;
        private Graphics graphics;
        private Bitmap bitmap;

        public static int ysize = 1025, xsize = ysize * 2 - 1;
        public static float[,] heightmap = new float[xsize, ysize];
        public static float roughness = 5f;

        

        //private static int _size = 1024;

        public PlotView()
        {
            InitializeComponent();

            CancellationTokenSource = new CancellationTokenSource();
            CancellationToken = CancellationTokenSource.Token;
        }

        private bool IsPow(int a)
        {
            return (a & (a - 1)) == 0;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();

            CancellationTokenSource = new CancellationTokenSource();
            CancellationToken = CancellationTokenSource.Token;
        }

        private async void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            //heightmap = DiamondSquareGrid(_size, 1, 255, 30);

            Generate();

            bitmap = new Bitmap(1024, 1024);
            graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Black);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //int scaleMin = 1;
            //int scaleMax = 255;

            //List<int> max = new List<int>();
            //List<int> min = new List<int>();

            //for (int i = 0; i < _size; i++)
            //{
            //    max.Add(heightmap[i].Max());
            //    min.Add(heightmap[i].Min());
            //}

            //int valueMax = max.Max();
            //int valueMin = min.Min();
            //int valueRange = valueMax - valueMin;
            //int scaleRange = scaleMax - scaleMin;

            //int[][] array = new int[_size][];

            //for (int i = 0; i < _size; i++)
            //{
            //    array[i] = new int[_size];
            //}

            //for (int i = 0; i < _size; i++)
            //{
            //    array[i] = array[i].Select(i => (scaleRange * (i - valueMin) / valueRange) + scaleMin).ToArray();
            //}

            await Task.Run(() =>
            {
                lock (graphics)
                {
                    for (int i = 0; i < xsize; i++)
                    {
                        for (int j = 0; j < ysize; j++)
                        {
                            int opacity = (int)Math.Round(heightmap[i, j] * 255); 
                            
                            if(opacity < 0)
                            {
                                opacity = 0;
                            }
                            else if (opacity > 255)
                            {
                                opacity = 255;
                            }
                       
                            Console.Write($"{opacity} ");
                            var pen = new Pen(Color.FromArgb(opacity, 255, 255, 255), 1);
                            graphics.DrawRectangle(pen, i, j, 1, 1);

                            /*Dispatcher.BeginInvoke(new Action(() =>
                            {
                                FractalImage.Source = bitmap.GetImageSource();
                            }), System.Windows.Threading.DispatcherPriority.Background);*/
                        }
                    }
                    Temp.Normal normal_hlp = new Temp.Normal();
                    Bitmap normal = normal_hlp.calculate(bitmap);
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        FractalImage.Source = normal.GetImageSource();
                    }), System.Windows.Threading.DispatcherPriority.Background);
                }
            });
        }

        public static void Square(int lx, int ly, int rx, int ry)
        {
            int l = (rx - lx) / 2;

            float a = heightmap[lx, ly];
            float b = heightmap[lx, ry];
            float c = heightmap[rx, ry];
            float d = heightmap[rx, ly];
            int cex = lx + l;
            int cey = ly + l;

            heightmap[cex, cey] = (a + b + c + d) / 4 + random.RandRange(-l * 2 * roughness / ysize, l * 2 * roughness / ysize);
        }

        private static bool lrflag = false;

        public static void Diamond(int tgx, int tgy, int l)
        {
            float a, b, c, d;

            if (tgy - l >= 0)
                a = heightmap[tgx, tgy - l];
            else
                a = heightmap[tgx, ysize - l];

            if (tgx - l >= 0)
                b = heightmap[tgx - l, tgy];
            else
                if (lrflag)
                b = heightmap[xsize - l, tgy];
            else
                b = heightmap[ysize - l, tgy];

            if (tgy + l < ysize)
                c = heightmap[tgx, tgy + l];
            else
                c = heightmap[tgx, l];

            if (lrflag)
                if (tgx + l < xsize)
                    d = heightmap[tgx + l, tgy];
                else
                    d = heightmap[l, tgy];
            else
                if (tgx + l < ysize)
                d = heightmap[tgx + l, tgy];
            else
                d = heightmap[l, tgy];

            heightmap[tgx, tgy] = (a + b + c + d) / 4 + random.RandRange(-l * 2 * roughness / ysize, l * 2 * roughness / ysize);
        }

        public static void DiamondSquare(int lx, int ly, int rx, int ry)
        {
            int l = (rx - lx) / 2;

            Square(lx, ly, rx, ry);

            Diamond(lx, ly + l, l);
            Diamond(rx, ry - l, l);
            Diamond(rx - l, ry, l);
            Diamond(lx + l, ly, l);
        }

        public static void MidPointDisplacement(int lx, int ly, int rx, int ry)
        {
            int l = (rx - lx) / 2;
            if (l > 0)
            {
                float a = heightmap[lx, ly];
                float b = heightmap[lx, ry];
                float c = heightmap[rx, ry];
                float d = heightmap[rx, ly];

                int cex = lx + l;
                int cey = ly + l;

                heightmap[cex, cey] = (a + b + c + d) / 4 + random.RandRange(-l * 2 * roughness / xsize, l * 2 * roughness / xsize);

                heightmap[lx, cey] = (a + b) / 2 + random.RandRange(-l * 2 * roughness / xsize, l * 2 * roughness / xsize);
                heightmap[rx, cey] = (c + d) / 2 + random.RandRange(-l * 2 * roughness / xsize, l * 2 * roughness / xsize);
                heightmap[cex, ly] = (a + d) / 2 + random.RandRange(-l * 2 * roughness / xsize, l * 2 * roughness / xsize);
                heightmap[cex, ry] = (b + c) / 2 + random.RandRange(-l * 2 * roughness / xsize, l * 2 * roughness / xsize);

                MidPointDisplacement(lx, ly, cex, cey);
                MidPointDisplacement(lx, ly + l, lx + l, ry);
                MidPointDisplacement(cex, cey, rx, ry);
                MidPointDisplacement(lx + l, ly, rx, cey);
            }
        }

        public static void Generate()
        {
            heightmap[0, 0] = random.RandRange(0.3f, 0.6f);
            heightmap[0, ysize - 1] = random.RandRange(0.3f, 0.6f);
            heightmap[xsize - 1, ysize - 1] = random.RandRange(0.3f, 0.6f);
            heightmap[xsize - 1, 0] = random.RandRange(0.3f, 0.6f);

            heightmap[ysize - 1, ysize - 1] = random.RandRange(0.3f, 0.6f);
            heightmap[ysize - 1, 0] = random.RandRange(0.3f, 0.6f);

            for (int l = (ysize - 1) / 2; l > 0; l /= 2)
                for (int x = 0; x < xsize - 1; x += l)
                {
                    if (x >= ysize - l)
                        lrflag = true;
                    else
                        lrflag = false;

                    for (int y = 0; y < ysize - 1; y += l)
                        DiamondSquare(x, y, x + l, y + l);
                }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}