using Diamond_Square.Interfaces;

namespace Diamond_Square.Models
{
    public class HeightMapping : IHeightMapping
    {
        public static readonly int YSize = 1025;
        public static readonly int XSize = 1025;

        private static readonly float roughness = 5f;
        private float[,] heightmap;

        public HeightMapping()
        {
            heightmap = new float[XSize, YSize];
        }

        private void Diamond(int leftX, int leftY, int rightX, int rightY, IRandomGenerator randomGenerator)
        {
            int middle = (rightX - leftX) / 2;

            float a = heightmap[leftX, leftY];
            float b = heightmap[leftX, rightY];
            float c = heightmap[rightX, rightY];
            float d = heightmap[rightX, leftY];

            int centerX = leftX + middle;
            int centerY = leftY + middle;

            float value = randomGenerator.NextFloat(-middle * 2 * roughness / YSize, middle * 2 * roughness / YSize);

            heightmap[centerX, centerY] = (a + b + c + d) / 4 + value;
        }

        private void Square(int sideCenterX, int sideCenterY, int middle, IRandomGenerator randomGenerator)
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

            float value = randomGenerator.NextFloat(-middle * 2 * roughness / YSize, middle * 2 * roughness / YSize);

            heightmap[sideCenterX, sideCenterY] = (a + b + c + d) / 4 + value;
        }

        private void DiamondSquare(int leftX, int leftY, int rightX, int rightY, IRandomGenerator randomGenerator)
        {
            int middle = (rightX - leftX) / 2;

            Diamond(leftX, leftY, rightX, rightY, randomGenerator);

            Square(leftX, leftY + middle, middle, randomGenerator);
            Square(rightX, rightY - middle, middle, randomGenerator);
            Square(rightX - middle, rightY, middle, randomGenerator);
            Square(leftX + middle, leftY, middle, randomGenerator);
        }

        private void MiddlePointDisplacement(int middleX, int middleY, int rightX, int rightY, IRandomGenerator randomGenerator)
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

                heightmap[centerX, centerY] = (a + b + c + d) / 4 + randomGenerator.NextFloat(-middle * 2 * roughness / XSize, middle * 2 * roughness / XSize);

                heightmap[middleX, centerY] = (a + b) / 2 + randomGenerator.NextFloat(-middle * 2 * roughness / XSize, middle * 2 * roughness / XSize);
                heightmap[rightX, centerY] = (c + d) / 2 + randomGenerator.NextFloat(-middle * 2 * roughness / XSize, middle * 2 * roughness / XSize);
                heightmap[centerX, middleY] = (a + d) / 2 + randomGenerator.NextFloat(-middle * 2 * roughness / XSize, middle * 2 * roughness / XSize);
                heightmap[centerX, rightY] = (b + c) / 2 + randomGenerator.NextFloat(-middle * 2 * roughness / XSize, middle * 2 * roughness / XSize);

                MiddlePointDisplacement(middleX, middleY, centerX, centerY, randomGenerator);
                MiddlePointDisplacement(middleX, middleY + middle, middleX + middle, rightY, randomGenerator);
                MiddlePointDisplacement(centerX, centerY, rightX, rightY, randomGenerator);
                MiddlePointDisplacement(middleX + middle, middleY, rightX, centerY, randomGenerator);
            }
        }

        public float[,] GenerateHeightMap(IRandomGenerator randomGenerator)
        {
            heightmap[0, 0] = randomGenerator.NextFloat(0.3f, 0.6f);
            heightmap[0, YSize - 1] = randomGenerator.NextFloat(0.3f, 0.6f);
            heightmap[XSize - 1, YSize - 1] = randomGenerator.NextFloat(0.3f, 0.6f);
            heightmap[XSize - 1, 0] = randomGenerator.NextFloat(0.3f, 0.6f);

            for (int middle = (YSize - 1); middle > 0; middle /= 2)
            {
                for (int x = 0; x < XSize - 1; x += middle)
                {
                    for (int y = 0; y < YSize - 1; y += middle)
                    {
                        DiamondSquare(x, y, x + middle, y + middle, randomGenerator);
                    }
                }
            }

            return heightmap;
        }
    }
}