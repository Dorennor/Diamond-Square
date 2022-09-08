using VR.DiamondSquare.Model.Interfaces;

namespace VR.DiamondSquare.Model.Models;

public class QuickNormalizator : INormalizator
{
    private readonly int _min;
    private readonly int _max;

    /// <summary>
    /// Takes min and max values for future normalization with Normalize method.
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public QuickNormalizator(int min, int max)
    {
        _min = min;
        _max = max;
    }

    /// <summary>
    /// Takes float square array with not unnormalized height map.
    /// Normalize it with the method of cutting off values.
    /// Returns new normalized float array.
    /// </summary>
    /// <param name="heightMap"></param>
    public float[,] Normalize(float[,] heightMap)
    {
        float[,] resultHeightMap = new float[heightMap.GetLength(0), heightMap.GetLength(1)];

        for (int i = 0; i < heightMap.GetLength(0); i++)
        {
            for (int j = 0; j < heightMap.GetLength(1); j++)
            {
                resultHeightMap[i, j] = heightMap[i, j] * 255;

                if (resultHeightMap[i, j] < _min)
                {
                    resultHeightMap[i, j] = _min;
                }
                else if (resultHeightMap[i, j] > _max)
                {
                    resultHeightMap[i, j] = _max;
                }
            }
        }

        return resultHeightMap;
    }
}