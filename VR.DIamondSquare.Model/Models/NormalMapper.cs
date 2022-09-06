using System.Drawing;
using VR.DiamondSquare.View.Interfaces;

namespace VR.DiamondSquare.View.Models
{
    public class NormalMapper : INormalMapper
    {
        public Bitmap GenerateNormalMap(Bitmap image, bool isGreyPalette)
        {
            int width = image.Width - 1;
            int height = image.Height - 1;

            float leftSample;
            float rightSample;
            float topSample;
            float bottomSample;

            float xVector;
            float yVector;

            Bitmap normalMap = new Bitmap(image.Width, image.Height);

            if (isGreyPalette)
            {
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

                        float zVector = (xVector + yVector) / 2;
                        Color color = Color.FromArgb(255, (int)xVector, (int)yVector, (int)zVector);

                        normalMap.SetPixel(x, y, color);
                    }
                }
            }
            else
            {
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

                        Color color = Color.FromArgb(255, (int)xVector, (int)yVector, 255);

                        normalMap.SetPixel(x, y, color);
                    }
                }
            }

            return normalMap;
        }
    }
}