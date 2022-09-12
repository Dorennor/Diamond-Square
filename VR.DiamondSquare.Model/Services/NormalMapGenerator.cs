using VR.DiamondSquare.Model.Interfaces;
using VR.DiamondSquare.Model.Models;

namespace VR.DiamondSquare.Model.Services;

public class NormalMapGenerator : INormalMapGenerator
{
    private readonly INormalizator _normalizator;
    private static readonly float Coefficient = 1f;

    public NormalMapGenerator(INormalizator normalizator)
    {
        _normalizator = normalizator;
    }

    /// <summary>
    /// Takes height map, that need to be already normalized (values from 0 to 255).
    /// Returns GreyNormalMap object with X, Y and Z float arrays, that contain values for R, G, B colors respectively for drawing it outside of model.
    /// </summary>
    /// <param name="heightMap"></param>
    public AlternativeNormalMap GenerateAlternativeNormalMap(float[,] heightMap)
    {
        AlternativeNormalMap result = new AlternativeNormalMap(heightMap.GetLength(0));

        int size = heightMap.GetLength(0) - 1;

        float leftSample;
        float rightSample;
        float topSample;
        float bottomSample;

        float xVector;
        float yVector;
        float zVector;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (x > 0)
                {
                    leftSample = heightMap[x - 1, y];
                }
                else
                {
                    leftSample = heightMap[x, y];
                }

                if (x < size)
                {
                    rightSample = heightMap[x + 1, y];
                }
                else
                {
                    rightSample = heightMap[x, y];
                }

                if (y > 1)
                {
                    topSample = heightMap[x, y - 1];
                }
                else
                {
                    topSample = heightMap[x, y];
                }

                if (y < size)
                {
                    bottomSample = heightMap[x, y + 1];
                }
                else
                {
                    bottomSample = heightMap[x, y];
                }

                xVector = (leftSample - rightSample) * Coefficient;
                yVector = (topSample - bottomSample) * Coefficient;
                zVector = (xVector + yVector) / 2;

                result.XVector[x, y] = xVector;
                result.YVector[y, x] = yVector;
                result.ZVector[x, y] = zVector;
            }
        }

        _normalizator.Normalize(result.XVector);
        _normalizator.Normalize(result.YVector);
        _normalizator.Normalize(result.ZVector);

        return result;
    }

    /// <summary>
    /// Takes height map, that need to be already normalized(values from 0 to 255).
    /// Returns NormalMap object with X and Y float arrays, that contain values for R, and G colors respectively for drawing it outside of model.
    /// </summary>
    /// <param name="heightMap"></param>
    public NormalMap GenerateNormalMap(float[,] heightMap)
    {
        NormalMap result = new NormalMap(heightMap.GetLength(0));

        int size = heightMap.GetLength(0) - 1;

        float leftSample;
        float rightSample;
        float topSample;
        float bottomSample;

        float xVector;
        float yVector;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (x > 0)
                {
                    leftSample = heightMap[x - 1, y];
                }
                else
                {
                    leftSample = heightMap[x, y];
                }

                if (x < size)
                {
                    rightSample = heightMap[x + 1, y];
                }
                else
                {
                    rightSample = heightMap[x, y];
                }

                if (y > 1)
                {
                    topSample = heightMap[x, y - 1];
                }
                else
                {
                    topSample = heightMap[x, y];
                }

                if (y < size)
                {
                    bottomSample = heightMap[x, y + 1];
                }
                else
                {
                    bottomSample = heightMap[x, y];
                }

                xVector = (leftSample - rightSample) * Coefficient;
                yVector = (topSample - bottomSample) * Coefficient;

                result.XVector[x, y] = xVector;
                result.YVector[y, x] = yVector;
            }
        }

        _normalizator.Normalize(result.XVector);
        _normalizator.Normalize(result.YVector);

        return result;
    }
}