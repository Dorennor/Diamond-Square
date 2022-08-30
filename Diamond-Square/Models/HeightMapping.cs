using System;
using Diamond_Square.Extensions;

namespace Diamond_Square.Models
{
    public static class HeightMapping
    {
        public static int YSize { get; } = 1025;
        public static int XSize { get; } = 1025;
        private static float[,] heightmap = new float[XSize, YSize];
        private static float roughness = 5f;

        public static void Square(int leftX, int leftY, int rightX, int rightY, Random random)
        {
            int middle = (rightX - leftX) / 2;

            float a = heightmap[leftX, leftY];
            float b = heightmap[leftX, rightY];
            float c = heightmap[rightX, rightY];
            float d = heightmap[rightX, leftY];

            int centerX = leftX + middle;
            int centerY = leftY + middle;

            heightmap[centerX, centerY] = (a + b + c + d) / 4 + random.RandRange(-middle * 2 * roughness / YSize, middle * 2 * roughness / YSize);
        }

        public static void Diamond(int sideCenterX, int sideCenterY, int middle, Random random)
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

            heightmap[sideCenterX, sideCenterY] = (a + b + c + d) / 4 + random.RandRange(-middle * 2 * roughness / YSize, middle * 2 * roughness / YSize);
        }

        public static void DiamondSquare(int leftX, int leftY, int rightX, int rightY, Random random)
        {
            int middle = (rightX - leftX) / 2;

            Square(leftX, leftY, rightX, rightY, random);

            Diamond(leftX, leftY + middle, middle, random);
            Diamond(rightX, rightY - middle, middle, random);
            Diamond(rightX - middle, rightY, middle, random);
            Diamond(leftX + middle, leftY, middle, random);
        }

        public static void MiddlePointDisplacement(int middleX, int middleY, int rightX, int rightY, Random random)
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

                heightmap[centerX, centerY] = (a + b + c + d) / 4 + random.RandRange(-middle * 2 * roughness / XSize, middle * 2 * roughness / XSize);

                heightmap[middleX, centerY] = (a + b) / 2 + random.RandRange(-middle * 2 * roughness / XSize, middle * 2 * roughness / XSize);
                heightmap[rightX, centerY] = (c + d) / 2 + random.RandRange(-middle * 2 * roughness / XSize, middle * 2 * roughness / XSize);
                heightmap[centerX, middleY] = (a + d) / 2 + random.RandRange(-middle * 2 * roughness / XSize, middle * 2 * roughness / XSize);
                heightmap[centerX, rightY] = (b + c) / 2 + random.RandRange(-middle * 2 * roughness / XSize, middle * 2 * roughness / XSize);

                MiddlePointDisplacement(middleX, middleY, centerX, centerY, random);
                MiddlePointDisplacement(middleX, middleY + middle, middleX + middle, rightY, random);
                MiddlePointDisplacement(centerX, centerY, rightX, rightY, random);
                MiddlePointDisplacement(middleX + middle, middleY, rightX, centerY, random);
            }
        }

        public static float[,] Generate(Random random)
        {
            heightmap[0, 0] = random.RandRange(0.3f, 0.6f);
            heightmap[0, YSize - 1] = random.RandRange(0.3f, 0.6f);
            heightmap[XSize - 1, YSize - 1] = random.RandRange(0.3f, 0.6f);
            heightmap[XSize - 1, 0] = random.RandRange(0.3f, 0.6f);

            heightmap[YSize - 1, YSize - 1] = random.RandRange(0.3f, 0.6f);
            heightmap[YSize - 1, 0] = random.RandRange(0.3f, 0.6f);

            for (int middle = (YSize - 1) / 2; middle > 0; middle /= 2)
            {
                for (int x = 0; x < XSize - 1; x += middle)
                {
                    for (int y = 0; y < YSize - 1; y += middle)
                    {
                        DiamondSquare(x, y, x + middle, y + middle, random);
                    }
                }
            }

            return heightmap;
        }
    }
}