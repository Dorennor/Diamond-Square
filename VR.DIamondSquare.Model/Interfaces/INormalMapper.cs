using System.Drawing;

namespace VR.DiamondSquare.View.Interfaces
{
    public interface INormalMapper
    {
        Bitmap GenerateNormalMap(Bitmap image, bool isGreyPalette);
    }
}