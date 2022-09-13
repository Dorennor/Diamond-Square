using VR.DiamondSquare.Model.Interfaces;

namespace VR.DiamondSquare.Model.Services;

public class HeightMapGenerator : IHeightMapGenerator
{
    public int Size { get; private set; }

    private readonly INormalizator _normalizator;
    private float[,] _heightMap;
    private float _seedMin;
    private float _seedMax;
    private float _attenuationCoefficient = 0.5f;

    public HeightMapGenerator(INormalizator normalizator)
    {
        _normalizator = normalizator;
    }

    /// <summary>
    /// Method calculate diamond points, when method is called heightMap already must be initialized.
    /// </summary>
    /// <param name="leftX"></param>
    /// <param name="leftY"></param>
    /// <param name="rightX"></param>
    /// <param name="rightY"></param>
    /// <param name="randomGenerator"></param>
    private void Diamond(int leftX, int leftY, int rightX, int rightY, IRandomGenerator randomGenerator)
    {
        int middle = (rightX - leftX) / 2;

        float a = _heightMap[leftX, leftY];
        float b = _heightMap[leftX, rightY];
        float c = _heightMap[rightX, rightY];
        float d = _heightMap[rightX, leftY];

        int centerX = leftX + middle;
        int centerY = leftY + middle;

        _heightMap[centerX, centerY] = (a + b + c + d) / 4 + randomGenerator.NextFloat(_seedMin, _seedMax);
    }

    /// <summary>
    /// Method calculate square points, when method is called heightMap already must be initialized.
    /// Also method must be called after call of Diamond method.
    /// Calculate square points from 4 neighboring points in case, if all of them > 0, otherwise use 3 non zero points.
    /// </summary>
    /// <param name="sideCenterX"></param>
    /// <param name="sideCenterY"></param>
    /// <param name="middle"></param>
    /// <param name="randomGenerator"></param>
    private void Square(int sideCenterX, int sideCenterY, int middle, IRandomGenerator randomGenerator)
    {
        float a, b, c, d;

        if (sideCenterY - middle >= 0)
        {
            a = _heightMap[sideCenterX, sideCenterY - middle];
        }
        else
        {
            a = _heightMap[sideCenterX, Size - middle];
        }

        if (sideCenterX - middle >= 0)
        {
            b = _heightMap[sideCenterX - middle, sideCenterY];
        }
        else
        {
            b = _heightMap[Size - middle, sideCenterY];
        }

        if (sideCenterY + middle < Size)
        {
            c = _heightMap[sideCenterX, sideCenterY + middle];
        }
        else
        {
            c = _heightMap[sideCenterX, middle];
        }

        if (sideCenterX + middle < Size)
        {
            d = _heightMap[sideCenterX + middle, sideCenterY];
        }
        else
        {
            d = _heightMap[middle, sideCenterY];
        }

        if (sideCenterX + middle < Size)
        {
            d = _heightMap[sideCenterX + middle, sideCenterY];
        }
        else
        {
            d = _heightMap[middle, sideCenterY];
        }

        if (a == 0)
        {
            _heightMap[sideCenterX, sideCenterY] = (b + c + d) / 3 + randomGenerator.NextFloat(_seedMin, _seedMax);
        }
        else if (b == 0)
        {
            _heightMap[sideCenterX, sideCenterY] = (a + c + d) / 3 + randomGenerator.NextFloat(_seedMin, _seedMax);
        }
        else if (c == 0)
        {
            _heightMap[sideCenterX, sideCenterY] = (a + b + d) / 3 + randomGenerator.NextFloat(_seedMin, _seedMax);
        }
        else if (d == 0)
        {
            _heightMap[sideCenterX, sideCenterY] = (a + b + c) / 3 + randomGenerator.NextFloat(_seedMin, _seedMax);
        }
        else
        {
            _heightMap[sideCenterX, sideCenterY] = (a + b + c + d) / 4 + randomGenerator.NextFloat(_seedMin, _seedMax);
        }
    }

    /// <summary>
    /// Method calls another method of algorithm.
    /// </summary>
    /// <param name="leftX"></param>
    /// <param name="leftY"></param>
    /// <param name="rightX"></param>
    /// <param name="rightY"></param>
    /// <param name="randomGenerator"></param>
    private void DiamondSquare(int leftX, int leftY, int rightX, int rightY, IRandomGenerator randomGenerator)
    {
        int stepLength = (rightX - leftX) / 2;

        Diamond(leftX, leftY, rightX, rightY, randomGenerator);

        Square(leftX, leftY + stepLength, stepLength, randomGenerator);
        Square(rightX, rightY - stepLength, stepLength, randomGenerator);
        Square(rightX - stepLength, rightY, stepLength, randomGenerator);
        Square(leftX + stepLength, leftY, stepLength, randomGenerator);
    }

    /// <summary>
    /// Initialize fields and run main cycle of algorithm. Size must be bigger than zero,
    /// min/max parameters need to be in approximately similar ranges, otherwise there will be inadequate results.
    /// </summary>
    /// <param name="randomGenerator"></param>
    /// <param name="size"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="seedMin"></param>
    /// <param name="seedMax"></param>
    /// <returns></returns>
    public float[,] GenerateHeightMap(IRandomGenerator randomGenerator, int size, float min, float max, float seedMin, float seedMax)
    {
        _seedMin = seedMin;
        _seedMax = seedMax;
        Size = size;

        _heightMap = new float[Size, Size];

        _heightMap[0, 0] = randomGenerator.NextFloat(min, max);
        _heightMap[0, Size - 1] = randomGenerator.NextFloat(min, max);
        _heightMap[Size - 1, Size - 1] = randomGenerator.NextFloat(min, max);
        _heightMap[Size - 1, 0] = randomGenerator.NextFloat(min, max);

        for (int middle = Size - 1; middle > 0; middle /= 2)
        {
            for (int x = 0; x < Size - 1; x += middle)
            {
                for (int y = 0; y < Size - 1; y += middle)
                {
                    DiamondSquare(x, y, x + middle, y + middle, randomGenerator);

                    _seedMin *= _attenuationCoefficient;
                    _seedMax *= _attenuationCoefficient;
                }
            }
        }

        _normalizator.Normalize(_heightMap);

        return _heightMap;
    }
}