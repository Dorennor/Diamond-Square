using System.Drawing;
using VR.DiamondSquare.View.Interfaces;

namespace VR.DiamondSquare.View.Models
{
    public class HeightGenerator : IHeightGenerator
    {
        public static readonly int Size = 1025;

        private static readonly float _min = -1;
        private static readonly float _max = 1;

        private IRandomSeedGenerator _randomSeed;
        private INormalizator _normalizator;
        private float[,] heightMap;

        public HeightGenerator()
        {
            heightMap = new float[Size, Size];
        }

        private void Diamond(int leftX, int leftY, int rightX, int rightY)
        {
            int middle = (rightX - leftX) / 2;

            float a = heightMap[leftX, leftY];
            float b = heightMap[leftX, rightY];
            float c = heightMap[rightX, rightY];
            float d = heightMap[rightX, leftY];

            int centerX = leftX + middle;
            int centerY = leftY + middle;

            heightMap[centerX, centerY] = ((a + b + c + d) / 4) + _randomSeed.GetRandomSeed(_min, _max);
        }

        private void Square(int sideCenterX, int sideCenterY, int middle)
        {
            float a, b, c, d;

            if (sideCenterY - middle >= 0)
            {
                a = heightMap[sideCenterX, sideCenterY - middle];
            }
            else
            {
                a = heightMap[sideCenterX, Size - middle];
            }

            if (sideCenterX - middle >= 0)
            {
                b = heightMap[sideCenterX - middle, sideCenterY];
            }
            else
            {
                b = heightMap[Size - middle, sideCenterY];
            }

            if (sideCenterY + middle < Size)
            {
                c = heightMap[sideCenterX, sideCenterY + middle];
            }
            else
            {
                c = heightMap[sideCenterX, middle];
            }

            if (sideCenterX + middle < Size)
            {
                d = heightMap[sideCenterX + middle, sideCenterY];
            }
            else
            {
                d = heightMap[middle, sideCenterY];
            }

            if (sideCenterX + middle < Size)
            {
                d = heightMap[sideCenterX + middle, sideCenterY];
            }
            else
            {
                d = heightMap[middle, sideCenterY];
            }

            heightMap[sideCenterX, sideCenterY] = ((a + b + c + d) / 4) + _randomSeed.GetRandomSeed(_min, _max);
        }

        private void DiamondSquare(int leftX, int leftY, int rightX, int rightY, IRandomGenerator randomGenerator)
        {
            int middle = (rightX - leftX) / 2;

            Diamond(leftX, leftY, rightX, rightY);

            Square(leftX, leftY + middle, middle);
            Square(rightX, rightY - middle, middle);
            Square(rightX - middle, rightY, middle);
            Square(leftX + middle, leftY, middle);
        }

        private void MiddlePointDisplacement(int middleX, int middleY, int rightX, int rightY)
        {
            int middle = (rightX - middleX) / 2;

            if (middle > 0)
            {
                float a = heightMap[middleX, middleY];
                float b = heightMap[middleX, rightY];
                float c = heightMap[rightX, rightY];
                float d = heightMap[rightX, middleY];

                int centerX = middleX + middle;
                int centerY = middleY + middle;

                heightMap[centerX, centerY] = ((a + b + c + d) / 4) + _randomSeed.GetRandomSeed(_min, _max);
                heightMap[middleX, centerY] = ((a + b) / 2) + _randomSeed.GetRandomSeed(_min, _max);
                heightMap[rightX, centerY] = ((c + d) / 2) + _randomSeed.GetRandomSeed(_min, _max);
                heightMap[centerX, middleY] = ((a + d) / 2) + _randomSeed.GetRandomSeed(_min, _max);
                heightMap[centerX, rightY] = ((b + c) / 2) + _randomSeed.GetRandomSeed(_min, _max);

                MiddlePointDisplacement(middleX, middleY, centerX, centerY);
                MiddlePointDisplacement(middleX, middleY + middle, middleX + middle, rightY);
                MiddlePointDisplacement(centerX, centerY, rightX, rightY);
                MiddlePointDisplacement(middleX + middle, middleY, rightX, centerY);
            }
        }

        public Bitmap GenerateHeightMap(IRandomGenerator randomGenerator)
        {
            _randomSeed = new RandomSeedGenerator(randomGenerator);
            _normalizator = new Normalizator(0, 255);

            heightMap[0, 0] = randomGenerator.NextFloat(1f, 10f);
            heightMap[0, Size - 1] = randomGenerator.NextFloat(1f, 10f);
            heightMap[Size - 1, Size - 1] = randomGenerator.NextFloat(1f, 10f);
            heightMap[Size - 1, 0] = randomGenerator.NextFloat(1f, 10f);

            for (int middle = Size - 1; middle > 0; middle /= 2)
            {
                for (int x = 0; x < Size - 1; x += middle)
                {
                    for (int y = 0; y < Size - 1; y += middle)
                    {
                        DiamondSquare(x, y, x + middle, y + middle, randomGenerator);
                    }
                }
            }

            int[,] normalizedHeightMap = _normalizator.Normalize(heightMap);

            Bitmap _bitmap = new Bitmap(Size, Size);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _bitmap.SetPixel(i, j, Color.FromArgb(255, normalizedHeightMap[i, j], normalizedHeightMap[i, j], normalizedHeightMap[i, j]));
                }
            }

            return _bitmap;
        }
    }
}