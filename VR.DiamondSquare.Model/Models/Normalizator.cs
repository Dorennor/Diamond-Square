using VR.DiamondSquare.Model.Interfaces;

namespace VR.DiamondSquare.Model.Models;

public class Normalizator : INormalizator
{
    private readonly int _min;
    private readonly int _max;

    /// <summary>
    /// Takes min and max values for future normalization with Normalize method.
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public Normalizator(int min, int max)
    {
        _min = min;
        _max = max;
    }

    /// <summary>
    /// Takes float square array with not unnormalized height map.
    /// Normalize it with MinMax algorithm.
    /// Returns new normalized float array.
    /// </summary>
    /// <param name="heightMap"></param>
    public float[,] Normalize(float[,] heightMap)
    {
        float[,] resultHeightMap = new float[heightMap.GetLength(0), heightMap.GetLength(1)];

        float minValue = heightMap.Cast<float>().Min();
        float maxValue = heightMap.Cast<float>().Max(); ;

        for (int i = 0; i < heightMap.GetLength(0); i++)
        {
            for (int j = 0; j < heightMap.GetLength(1); j++)
            {
                resultHeightMap[i, j] = (heightMap[i, j] - minValue) / (maxValue - minValue) * 255;
            }
        }

        return resultHeightMap;
    }
}