using VR.DiamondSquare.Model.Interfaces;

namespace VR.DiamondSquare.Model.Services;

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
    public void Normalize(float[,] heightMap)
    {
        for (int i = 0; i < heightMap.GetLength(0); i++)
        {
            for (int j = 0; j < heightMap.GetLength(1); j++)
            {
                heightMap[i, j] = heightMap[i, j] * 255;

                if (heightMap[i, j] < _min)
                {
                    heightMap[i, j] = _min;
                }
                else if (heightMap[i, j] > _max)
                {
                    heightMap[i, j] = _max;
                }
            }
        }
    }
}