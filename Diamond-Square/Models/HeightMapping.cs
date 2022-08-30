using System;
using Diamond_Square.Extensions;

namespace Diamond_Square.Models
{
    public static class HeightMapping
    {
        public static int YSize { get; } = 1025;
        public static int XSize { get; } = YSize * 2 - 1;
        private static float[,] heightmap = new float[XSize, YSize];
        private static float roughness = 5f;
        private static bool lrflag = false;

        public static void Square(int lx, int ly, int rx, int ry, Random random)
        {
            int l = (rx - lx) / 2;

            float a = heightmap[lx, ly];
            float b = heightmap[lx, ry];
            float c = heightmap[rx, ry];
            float d = heightmap[rx, ly];
            int cex = lx + l;
            int cey = ly + l;

            heightmap[cex, cey] = (a + b + c + d) / 4 + random.RandRange(-l * 2 * roughness / YSize, l * 2 * roughness / YSize);
        }

        public static void Diamond(int tgx, int tgy, int l, Random random)
        {
            float a, b, c, d;

            if (tgy - l >= 0)
            {
                a = heightmap[tgx, tgy - l];
            }
            else
            {
                a = heightmap[tgx, YSize - l];
            }

            if (tgx - l >= 0)
            {
                b = heightmap[tgx - l, tgy];
            }
            else
            {
                if (lrflag)
                {
                    b = heightmap[XSize - l, tgy];
                }
                else
                {
                    b = heightmap[YSize - l, tgy];
                }
            }

            if (tgy + l < YSize)
            {
                c = heightmap[tgx, tgy + l];
            }
            else
            {
                c = heightmap[tgx, l];
            }

            if (lrflag)
            {
                if (tgx + l < XSize)
                {
                    d = heightmap[tgx + l, tgy];
                }
                else
                {
                    d = heightmap[l, tgy];
                }
            }
            else
            {
                if (tgx + l < YSize)
                {
                    d = heightmap[tgx + l, tgy];
                }
                else
                {
                    d = heightmap[l, tgy];
                }
            }
            heightmap[tgx, tgy] = (a + b + c + d) / 4 + random.RandRange(-l * 2 * roughness / YSize, l * 2 * roughness / YSize);
        }

        public static void DiamondSquare(int lx, int ly, int rx, int ry, Random random)
        {
            int l = (rx - lx) / 2;

            Square(lx, ly, rx, ry, random);

            Diamond(lx, ly + l, l, random);
            Diamond(rx, ry - l, l, random);
            Diamond(rx - l, ry, l, random);
            Diamond(lx + l, ly, l, random);
        }

        public static void MidPointDisplacement(int lx, int ly, int rx, int ry, Random random)
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

                heightmap[cex, cey] = (a + b + c + d) / 4 + random.RandRange(-l * 2 * roughness / XSize, l * 2 * roughness / XSize);

                heightmap[lx, cey] = (a + b) / 2 + random.RandRange(-l * 2 * roughness / XSize, l * 2 * roughness / XSize);
                heightmap[rx, cey] = (c + d) / 2 + random.RandRange(-l * 2 * roughness / XSize, l * 2 * roughness / XSize);
                heightmap[cex, ly] = (a + d) / 2 + random.RandRange(-l * 2 * roughness / XSize, l * 2 * roughness / XSize);
                heightmap[cex, ry] = (b + c) / 2 + random.RandRange(-l * 2 * roughness / XSize, l * 2 * roughness / XSize);

                MidPointDisplacement(lx, ly, cex, cey, random);
                MidPointDisplacement(lx, ly + l, lx + l, ry, random);
                MidPointDisplacement(cex, cey, rx, ry, random);
                MidPointDisplacement(lx + l, ly, rx, cey, random);
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

            for (int l = (YSize - 1) / 2; l > 0; l /= 2)
            {
                for (int x = 0; x < XSize - 1; x += l)
                {
                    if (x >= YSize - l)
                    {
                        lrflag = true;
                    }
                    else
                    {
                        lrflag = false;
                    }

                    for (int y = 0; y < YSize - 1; y += l)
                    {
                        DiamondSquare(x, y, x + l, y + l, random);
                    }
                }
            }

            return heightmap;
        }
    }
}