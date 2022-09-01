using Diamond_Square.Interfaces;

namespace Diamond_Square.Models
{
    public class HeightMapping : IHeightMapping
    {
        public static readonly int YSize = 1025;
        public static readonly int XSize = 1025;

        private static readonly float _roughness = 5f;
        private static readonly float _min = -1;
        private static readonly float _max = 1;

        private IRandomSeed _randomSeed;
        private INormalization _normalizer;
        private float[,] heightmap;

        public HeightMapping()
        {
            heightmap = new float[XSize, YSize];
        }

        private void Diamond(int leftX, int leftY, int rightX, int rightY)
        {
            int middle = (rightX - leftX) / 2;

            float a = heightmap[leftX, leftY];
            float b = heightmap[leftX, rightY];
            float c = heightmap[rightX, rightY];
            float d = heightmap[rightX, leftY];

            int centerX = leftX + middle;
            int centerY = leftY + middle;

            heightmap[centerX, centerY] = ((a + b + c + d) / 4) + _randomSeed.GetRandomSeed(_min, _max);
        }

        private void Square(int sideCenterX, int sideCenterY, int middle)
        {
            float a, b, c, d;

            if (sideCenterY - middle >= 0)
            {
                a = heightmap[sideCenterX, sideCenterY - middle];
            }
            else
            {
                a = heightmap[sideCenterX, YSize - middle];
            }

            if (sideCenterX - middle >= 0)
            {
                b = heightmap[sideCenterX - middle, sideCenterY];
            }
            else
            {
                b = heightmap[XSize - middle, sideCenterY];
            }

            if (sideCenterY + middle < YSize)
            {
                c = heightmap[sideCenterX, sideCenterY + middle];
            }
            else
            {
                c = heightmap[sideCenterX, middle];
            }

            if (sideCenterX + middle < XSize)
            {
                d = heightmap[sideCenterX + middle, sideCenterY];
            }
            else
            {
                d = heightmap[middle, sideCenterY];
            }

            if (sideCenterX + middle < YSize)
            {
                d = heightmap[sideCenterX + middle, sideCenterY];
            }
            else
            {
                d = heightmap[middle, sideCenterY];
            }

            heightmap[sideCenterX, sideCenterY] = ((a + b + c + d) / 4) + _randomSeed.GetRandomSeed(_min, _max);
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
                float a = heightmap[middleX, middleY];
                float b = heightmap[middleX, rightY];
                float c = heightmap[rightX, rightY];
                float d = heightmap[rightX, middleY];

                int centerX = middleX + middle;
                int centerY = middleY + middle;

                heightmap[centerX, centerY] = ((a + b + c + d) / 4) + _randomSeed.GetRandomSeed(_min, _max);
                heightmap[middleX, centerY] = ((a + b) / 2) + _randomSeed.GetRandomSeed(_min, _max);
                heightmap[rightX, centerY] = ((c + d) / 2) + _randomSeed.GetRandomSeed(_min, _max);
                heightmap[centerX, middleY] = ((a + d) / 2) + _randomSeed.GetRandomSeed(_min, _max);
                heightmap[centerX, rightY] = ((b + c) / 2) + _randomSeed.GetRandomSeed(_min, _max);

                MiddlePointDisplacement(middleX, middleY, centerX, centerY);
                MiddlePointDisplacement(middleX, middleY + middle, middleX + middle, rightY);
                MiddlePointDisplacement(centerX, centerY, rightX, rightY);
                MiddlePointDisplacement(middleX + middle, middleY, rightX, centerY);
            }
        }

        public int[,] GenerateHeightMap(IRandomGenerator randomGenerator)
        {
            _randomSeed = new RandomSeed(randomGenerator);
            _normalizer = new Normalizer(0, 255);

            heightmap[0, 0] = randomGenerator.NextFloat(0.3f, 0.6f);
            heightmap[0, YSize - 1] = randomGenerator.NextFloat(0.3f, 0.6f);
            heightmap[XSize - 1, YSize - 1] = randomGenerator.NextFloat(0.3f, 0.6f);
            heightmap[XSize - 1, 0] = randomGenerator.NextFloat(0.3f, 0.6f);

            for (int middle = YSize - 1; middle > 0; middle /= 2)
            {
                for (int x = 0; x < XSize - 1; x += middle)
                {
                    for (int y = 0; y < YSize - 1; y += middle)
                    {
                        DiamondSquare(x, y, x + middle, y + middle, randomGenerator);
                    }
                }
            }

            return _normalizer.Normalize(heightmap);
        }
    }
}