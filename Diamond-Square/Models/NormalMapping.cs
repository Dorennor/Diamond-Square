using System.Drawing;

namespace Diamond_Square.Models
{
    public static class NormalMapping
    {
        public static Bitmap Calculate(Bitmap image)
        {
            int width = image.Width - 1;
            int height = image.Height - 1;
            float leftSample;
            float rightSample;
            float topSample;
            float bottomSample;
            float xVector;
            float yVector;

            //Random random = new Random();

            Bitmap normalMap = new Bitmap(image.Width, image.Height);

            for (int y = 0; y <= width; y++)
            {
                for (int x = 0; x <= height; x++)
                {
                    if (x > 0)
                    {
                        leftSample = image.GetPixel(x - 1, y).GetBrightness();
                    }
                    else
                    {
                        leftSample = image.GetPixel(x, y).GetBrightness();
                    }

                    if (x < width)
                    {
                        rightSample = image.GetPixel(x + 1, y).GetBrightness();
                    }
                    else
                    {
                        rightSample = image.GetPixel(x, y).GetBrightness();
                    }

                    if (y > 1)
                    {
                        topSample = image.GetPixel(x, y - 1).GetBrightness();
                    }
                    else
                    {
                        topSample = image.GetPixel(x, y).GetBrightness();
                    }

                    if (y < height)
                    {
                        bottomSample = image.GetPixel(x, y + 1).GetBrightness();
                    }
                    else
                    {
                        bottomSample = image.GetPixel(x, y).GetBrightness();
                    }

                    xVector = (leftSample - rightSample + 1) * 0.5f * 255;
                    yVector = (topSample - bottomSample + 1) * 0.5f * 255;

                    Color color = Color.FromArgb(255, (int)xVector, (int)yVector, 255/*random.Next(1, 256)*/);

                    normalMap.SetPixel(x, y, color);
                }
            }

            return normalMap;
        }
    }
}