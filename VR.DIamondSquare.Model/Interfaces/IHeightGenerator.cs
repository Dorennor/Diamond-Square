using System.Drawing;

namespace VR.DiamondSquare.View.Interfaces
{
    public interface IHeightGenerator
    {
        Bitmap GenerateHeightMap(IRandomGenerator randomGenerator);
    }
}