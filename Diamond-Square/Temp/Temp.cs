using System;
using System.Drawing;
using Diamond_Square.Models;

namespace Diamond_Square.Temp
{
    internal class Temp
    {
        private const int _HIGH_RANGE = 1000;
        private const int _MAX_INDEX = 512;
        private const int _MAX_PALETTES = 5;
        private Random random = new Random();
        public static Color[][] Palette = new Color[_MAX_PALETTES][];


        private void Test()
        {
            //square.A.X = _minSize;
            //square.A.Y = _minSize;
            //square.A.Height = random.Next(1, 256);
            //Points.Add(square.A);

            //square.B.X = _minSize;
            //square.B.Y = _maxSize;
            //square.B.Height = random.Next(1, 256);
            //Points.Add(square.B);

            //square.C.X = _maxSize;
            //square.C.Y = _maxSize;
            //square.C.Height = random.Next(1, 256);
            //Points.Add(square.C);

            //square.D.X = _maxSize;
            //square.D.Y = _minSize;
            //square.D.Height = random.Next(1, 256);
            //Points.Add(square.D);

            //private void InitializeFractal()
            //{
            //    heightmap[_minSize, _minSize] = random.Next(1, 256);
            //    heightmap[_minSize, _maxSize] = random.Next(1, 256);
            //    heightmap[_maxSize, _minSize] = random.Next(1, 256);
            //    heightmap[_maxSize, _maxSize] = random.Next(1, 256);
            //}

            //private void CalculateDiamond(int minSize, int maxSize)
            //{
            //    heightmap[minSize / 2, maxSize / 2]
            //        = (heightmap[minSize, minSize]
            //        + heightmap[minSize, maxSize]
            //        + heightmap[maxSize, minSize]
            //        + heightmap[maxSize, maxSize])
            //        / 4;
            //}

            //private void DrawFractal()
            //{
            //    int minSize = _minSize, maxSize = _maxSize;

            //    for (int i = 0; i < _size; i++)
            //    {
            //        if (i == 0)
            //        {
            //            CalculateDiamond(minSize, maxSize);
            //            continue;
            //        }

            //        CalculateDiamond(minSize, maxSize / 2);

            //        CalculateDiamond(minSize, maxSize);

            //        CalculateDiamond(minSize, maxSize);

            //        CalculateDiamond(minSize, maxSize);
            //    }
            //}

            //square.M.X = square.C.X / 2;
            //square.M.Y = square.C.Y / 2;
            //square.M.Height = (square.A.Height + square.B.Height + square.C.Height + square.D.Height) / 4;
            //Points.Add(square.M);
        }
        private void Initialize(ref int[,] W)
        {
            W[0, 0] = random.Next() % _HIGH_RANGE;
            W[_MAX_INDEX, 0] = random.Next() % _HIGH_RANGE;
            W[0, _MAX_INDEX] = random.Next() % _HIGH_RANGE;
            W[_MAX_INDEX, _MAX_INDEX] = random.Next() % _HIGH_RANGE;
        }

        public int RandomNumber(int range, float S)
        {
            return (int)((random.NextDouble() * 2 - 1) * range * S);
        }

        private void DoDiamondSquare(ref int[,] W, float S)
        {
            int hs, x, y;
            int A, B, C, D, M, n;

            for (int it = _MAX_INDEX; it > 1; it /= 2)
            {
                hs = it / 2;

                for (y = hs; y < _MAX_INDEX; y += it)
                {
                    for (x = hs; x < _MAX_INDEX; x += it)
                    {
                        A = W[x - hs, y - hs];
                        B = W[x - hs, y + hs];
                        C = W[x + hs, y - hs];
                        D = W[x + hs, y + hs];

                        W[x, y] = ((A + B + C + D) / 4) + RandomNumber(hs, S);
                    }
                }

                for (y = 0; y < _MAX_INDEX + 1; y += hs)
                {
                    for (x = y % it == 0 ? hs : 0; x < _MAX_INDEX + 1; x += it)
                    {
                        M = n = 0;

                        try { M += W[x + hs, y]; n++; } catch (Exception) { }
                        try { M += W[x - hs, y]; n++; } catch (Exception) { }
                        try { M += W[x, y + hs]; n++; } catch (Exception) { }
                        try { M += W[x, y - hs]; n++; } catch (Exception) { }

                        W[x, y] = M / n + RandomNumber(hs, S) / 2;
                    }
                }
            }
        }

        private void CreatePalettes()
        {
            int i;

            Palette[0] = new Color[_HIGH_RANGE];

            for (i = 0; i < 1000; i++)
            {
                Palette[0][i] = Color.FromArgb(0xff, i % 255, i % 255, i % 255);
            }

            Palette[1] = new Color[_HIGH_RANGE];
            for (i = 0; i < 100; i++)
            {
                Palette[1][i] = Color.FromArgb(0x0f, 0, 0, random.Next() % 40 + 5);
            }

            for (i = 100; i < 300; i++)
            {
                Palette[1][i] = Color.FromArgb(0x0f, 0, 0, i / 2 - 50);
            }

            for (i = 300; i < 500; i++)
            {
                Palette[1][i] = Color.FromArgb(0x0f, i / 2 - 150, i - 300, 0);
            }

            for (i = 500; i < 700; i++)
            {
                Palette[1][i] = Color.FromArgb(0x0f, 0, 355 - i / 2, 0);
            }

            for (i = 700; i < 950; i++)
            {
                Palette[1][i] = Color.FromArgb(0x0f, i - 700, i - 700, i - 700);
            }

            for (i = 950; i < 1000; i++)
            {
                Palette[1][i] = Color.FromArgb(0xff, i - 800, i - 800, i - 800);
            }

            Palette[2] = new Color[_HIGH_RANGE];
            for (i = 0; i < 1000; i++)
            {
                Palette[2][i] = Color.FromArgb(0xff, (i / 4) + 5, (i / 4) + 5, 155);
            }

            Palette[3] = new Color[_HIGH_RANGE];
            for (i = 0; i < 1000; i++)
            {
                Palette[3][i] = Color.Black;
            }

            for (i = 0; i < 5; i++)
            {
                Palette[3][1 + 150 * i] = Palette[3][13 + 150 * i] = Color.DarkRed;
                Palette[3][2 + 150 * i] = Palette[3][12 + 150 * i] = Color.Red;
                Palette[3][3 + 150 * i] = Palette[3][11 + 150 * i] = Color.Red;
                Palette[3][4 + 150 * i] = Palette[3][10 + 150 * i] = Color.Orange;
                Palette[3][5 + 150 * i] = Palette[3][9 + 150 * i] = Color.Yellow;
                Palette[3][6 + 150 * i] = Palette[3][8 + 150 * i] = Color.LightYellow;
                Palette[3][7 + 150 * i] = Color.White;
            }

            Palette[4] = new Color[_HIGH_RANGE];
            for (i = 0; i < 1000; i++)
                Palette[4][i] = Color.FromArgb(0xff, 30, 30, (i / 5) + 50);

            for (i = 0; i < 4; i++)
            {
                Palette[4][1 + 150 * i] = Palette[4][13 + 150 * i] = Color.DeepSkyBlue;
                Palette[4][2 + 150 * i] = Palette[4][12 + 150 * i] = Color.SkyBlue;
                Palette[4][3 + 150 * i] = Palette[4][11 + 150 * i] = Color.Blue;
                Palette[4][4 + 150 * i] = Palette[4][10 + 150 * i] = Color.LightSteelBlue;
                Palette[4][5 + 150 * i] = Palette[4][9 + 150 * i] = Color.LightSteelBlue;
                Palette[4][6 + 150 * i] = Palette[4][8 + 150 * i] = Color.LightCyan;
                Palette[4][7 + 150 * i] = Color.White;
            }
        }
    }
}