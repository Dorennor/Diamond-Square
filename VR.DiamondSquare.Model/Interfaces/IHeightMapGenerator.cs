namespace VR.DiamondSquare.Model.Interfaces;

public interface IHeightMapGenerator
{
    float[,] GenerateHeightMap(IRandomGenerator randomGenerator, int size, float min, float max, float seedMin, float seedMax);
}