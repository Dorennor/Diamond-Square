﻿using VR.DiamondSquare.Model.Interfaces;
using VR.DiamondSquare.Model.Models;

namespace VR.DiamondSquare.Model.Services;

public class SobelNormalMapGenerator : INormalMapGenerator
{
    private readonly INormalizator _normalizator;
    private static readonly float Coefficient = 1f;

    //private readonly int[,] sobelX = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
    //private readonly int[,] sobelY = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
    //private readonly int[,] sobelZ = { { -1 - 2 - 1 }, { -2 - 4 - 2 }, { -1 - 2 - 1 } };

    private readonly int[,] sobelX = { { 3, 10, 3 }, { 0, 0, 0 }, { -3, -10, -3 } };
    private readonly int[,] sobelY = { { 3, 0, 3 }, { 10, 0, -10 }, { 3, 0, -3 } };
    //private readonly int[,] sobelZ = { { -1 - 2 - 1 }, { -2 - 4 - 2 }, { -1 - 2 - 1 } };

    public SobelNormalMapGenerator(INormalizator normalizator)
    {
        _normalizator = normalizator;
    }

    public AlternativeNormalMap GenerateAlternativeNormalMap(float[,] heightMap)
    {
        AlternativeNormalMap result = new AlternativeNormalMap(heightMap.GetLength(0));

        for (int i = 0; i < heightMap.GetLength(1); i++)
        {
            for (int j = 0; j < heightMap.GetLength(2); j++)
            {
                result.XVector[i, j] = 1;
                result.YVector[i, j] = 1;
            }
        }

        int size = heightMap.GetLength(0) - 1;

        float center, left, right, top, bottom, leftTop, rightTop, leftBottom, rightBottom;

        float xVector;
        float yVector;
        float zVector;

        for (int y = 1; y < size; y++)
        {
            for (int x = 1; x < size; x++)
            {
                center = heightMap[x, y];
                left = heightMap[x - 1, y];
                right = heightMap[x + 1, y];
                top = heightMap[x, y - 1];
                bottom = heightMap[x, y + 1];
                leftTop = heightMap[x - 1, y - 1];
                rightTop = heightMap[x + 1, y - 1];
                leftBottom = heightMap[x - 1, y + 1];
                rightBottom = heightMap[x + 1, y + 1];

                xVector = (leftTop * sobelX[0, 0]) +
                    (top * sobelX[0, 1]) +
                    (rightTop * sobelX[0, 2]) +
                    (left * sobelX[1, 0]) +
                    (center * sobelX[1, 1]) +
                    (right * sobelX[1, 2]) +
                    (leftBottom * sobelX[2, 0]) +
                    (bottom * sobelX[2, 1]) +
                    (rightBottom * sobelX[2, 2]);

                yVector = (leftTop * sobelY[0, 0]) +
                    (top * sobelY[0, 1]) +
                    (rightTop * sobelY[0, 2]) +
                    (left * sobelY[1, 0]) +
                    (center * sobelY[1, 1]) +
                    (right * sobelY[1, 2]) +
                    (leftBottom * sobelY[2, 0]) +
                    (bottom * sobelY[2, 1]) +
                    (rightBottom * sobelY[2, 2]);

                zVector = (xVector + yVector) / 2;

                result.XVector[x, y] = xVector * Coefficient;
                result.YVector[y, x] = yVector * Coefficient;
                result.ZVector[x, y] = zVector * Coefficient;
            }
        }

        _normalizator.Normalize(result.XVector);
        _normalizator.Normalize(result.YVector);
        _normalizator.Normalize(result.ZVector);

        return result;
    }

    public NormalMap GenerateNormalMap(float[,] heightMap)
    {
        NormalMap result = new NormalMap(heightMap.GetLength(0));

        for (int i = 0; i < heightMap.GetLength(0); i++)
        {
            for (int j = 0; j < heightMap.GetLength(1); j++)
            {
                result.XVector[i, j] = 1;
                result.YVector[i, j] = 1;
            }
        }

        int size = heightMap.GetLength(0) - 1;

        float center, left, right, top, bottom, leftTop, rightTop, leftBottom, rightBottom;

        float xVector;
        float yVector;

        for (int y = 1; y < size; y++)
        {
            for (int x = 1; x < size; x++)
            {
                center = heightMap[x, y];
                left = heightMap[x - 1, y];
                right = heightMap[x + 1, y];
                top = heightMap[x, y - 1];
                bottom = heightMap[x, y + 1];
                leftTop = heightMap[x - 1, y - 1];
                rightTop = heightMap[x + 1, y - 1];
                leftBottom = heightMap[x - 1, y + 1];
                rightBottom = heightMap[x + 1, y + 1];

                xVector = (leftTop * sobelX[0, 0]) +
                    (top * sobelX[0, 1]) +
                    (rightTop * sobelX[0, 2]) +
                    (left * sobelX[1, 0]) +
                    (center * sobelX[1, 1]) +
                    (right * sobelX[1, 2]) +
                    (leftBottom * sobelX[2, 0]) +
                    (bottom * sobelX[2, 1]) +
                    (rightBottom * sobelX[2, 2]);

                yVector = (leftTop * sobelY[0, 0]) +
                    (top * sobelY[0, 1]) +
                    (rightTop * sobelY[0, 2]) +
                    (left * sobelY[1, 0]) +
                    (center * sobelY[1, 1]) +
                    (right * sobelY[1, 2]) +
                    (leftBottom * sobelY[2, 0]) +
                    (bottom * sobelY[2, 1]) +
                    (rightBottom * sobelY[2, 2]);

                result.XVector[x, y] = xVector * Coefficient;
                result.YVector[y, x] = yVector * Coefficient;
            }
        }

        _normalizator.Normalize(result.XVector);
        _normalizator.Normalize(result.YVector);

        return result;
    }
}