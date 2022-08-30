using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond_Square.Temp
{
    public class Normal
    {
        public Bitmap calculate(Bitmap image)
        {
            #region Global Variables
            int w = image.Width - 1;
            int h = image.Height - 1;
            float sample_l;
            float sample_r;
            float sample_u;
            float sample_d;
            float x_vector;
            float y_vector;
            Bitmap normal = new Bitmap(image.Width, image.Height);
            #endregion
            for (int y = 0; y < w + 1; y++)
            {
                for (int x = 0; x < h + 1; x++)
                {
                    if (x > 0) { sample_l = image.GetPixel(x - 1, y).GetBrightness(); }
                    else { sample_l = image.GetPixel(x, y).GetBrightness(); }
                    if (x < w) { sample_r = image.GetPixel(x + 1, y).GetBrightness(); }
                    else { sample_r = image.GetPixel(x, y).GetBrightness(); }
                    if (y > 1) { sample_u = image.GetPixel(x, y - 1).GetBrightness(); }
                    else { sample_u = image.GetPixel(x, y).GetBrightness(); }
                    if (y < h) { sample_d = image.GetPixel(x, y + 1).GetBrightness(); }
                    else { sample_d = image.GetPixel(x, y).GetBrightness(); }
                    x_vector = (((sample_l - sample_r) + 1) * .5f) * 255;
                    y_vector = (((sample_u - sample_d) + 1) * .5f) * 255;
                    Color col = Color.FromArgb(255, (int)x_vector, (int)y_vector, 255);
                    normal.SetPixel(x, y, col);
                }
            }
            return normal;
        }
    }
}
