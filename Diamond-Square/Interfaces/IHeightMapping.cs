using System.Drawing;

namespace Diamond_Square.Interfaces
{
    public interface IHeightMapping
    {
        Bitmap GenerateHeightMap(IRandomGenerator randomGenerator);
    }
}