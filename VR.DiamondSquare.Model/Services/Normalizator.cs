using VR.DiamondSquare.Model.Interfaces;

namespace VR.DiamondSquare.Model.Services;

public class Normalizator : INormalizator
{
    /// <summary>
    /// Takes float square array with not unnormalized height map.
    /// Normalize it with MinMax algorithm.
    /// Returns new normalized float array with values between 0 and 1.
    /// </summary>
    /// <param name="heightMap"></param>
    public void Normalize(float[,] heightMap)
    {
        var heightMapEnumerable = heightMap.Cast<float>();

        float minValue = heightMapEnumerable.Min();
        float maxValue = heightMapEnumerable.Max(); ;

        for (int i = 0; i < heightMap.GetLength(0); i++)
        {
            for (int j = 0; j < heightMap.GetLength(1); j++)
            {
                heightMap[i, j] = (heightMap[i, j] - minValue) / (maxValue - minValue);
            }
        }
    }
}