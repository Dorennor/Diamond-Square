using System.Drawing;

namespace Diamond_Square.Models
{
    public static class NormalMapping
    {
        public static Bitmap Calculate(Bitmap image)
        {
            int width = image.Width - 1;
            int height = image.Height - 1;
            float sample_l;
            float sample_r;
            float sample_u;
            float sample_d;
            float xVector;
            float yVector;

            Bitmap normalMap = new Bitmap(image.Width, image.Height);

            for (int y = 0; y < width + 1; y++)
            {
                for (int x = 0; x < height + 1; x++)
                {
                    if (x > 0)
                    {
                        sample_l = image.GetPixel(x - 1, y).GetBrightness();
                    }
                    else
                    {
                        sample_l = image.GetPixel(x, y).GetBrightness();
                    }

                    if (x < width)
                    {
                        sample_r = image.GetPixel(x + 1, y).GetBrightness();
                    }
                    else
                    {
                        sample_r = image.GetPixel(x, y).GetBrightness();
                    }

                    if (y > 1)
                    {
                        sample_u = image.GetPixel(x, y - 1).GetBrightness();
                    }
                    else
                    {
                        sample_u = image.GetPixel(x, y).GetBrightness();
                    }

                    if (y < height)
                    {
                        sample_d = image.GetPixel(x, y + 1).GetBrightness();
                    }
                    else
                    {
                        sample_d = image.GetPixel(x, y).GetBrightness();
                    }

                    xVector = (sample_l - sample_r + 1) * 0.5f * 255;
                    yVector = (sample_u - sample_d + 1) * 0.5f * 255;

                    Color col = Color.FromArgb(255, (int)xVector, (int)yVector, 255);

                    normalMap.SetPixel(x, y, col);
                }
            }

            return normalMap;
        }
    }
}