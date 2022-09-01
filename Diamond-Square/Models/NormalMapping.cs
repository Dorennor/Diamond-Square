using System.Drawing;
using Diamond_Square.Interfaces;

namespace Diamond_Square.Models
{
    public class NormalMapping : INormalMapping
    {
        public Bitmap GenerateNormalMap(Bitmap image)
        {
            int width = image.Width - 1;
            int height = image.Height - 1;

            float leftSample;
            float rightSample;
            float topSample;
            float bottomSample;

            float xVector;
            float yVector;
            //float zVector;

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

                    float coefficient = 0.5f;

                    xVector = (leftSample - rightSample + 1) * coefficient * 255;
                    yVector = (topSample - bottomSample + 1) * coefficient * 255;
                    //zVector = (xVector + yVector) / 2;

                    Color color = Color.FromArgb(255, (int)xVector, (int)yVector, 255/*(int)zVector*/);

                    normalMap.SetPixel(x, y, color);
                }
            }

            return normalMap;
        }
    }
}