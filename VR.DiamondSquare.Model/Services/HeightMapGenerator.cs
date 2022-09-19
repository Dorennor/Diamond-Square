using System.Drawing;
using System.Runtime.CompilerServices;
using VR.DiamondSquare.Model.Interfaces;

namespace VR.DiamondSquare.Model.Services;

public class HeightMapGenerator : IHeightMapGenerator
{
    private readonly INormalizator _normalizator;
    private readonly float _attenuationCoefficient = 0.5f;

    private bool[,] _flags;
    private float[,] _heightMap;

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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Diamond(Point left, Point right, Point result, IRandomGenerator randomGenerator, float min, float max)
    {
        if (_flags[result.X, result.Y])
        {
            return;
        }
        else
        {
            _flags[result.X, result.Y] = true;
        }

        float a = _heightMap[left.X, left.Y];
        float b = _heightMap[left.X, right.Y];
        float c = _heightMap[right.X, right.Y];
        float d = _heightMap[right.X, left.Y];

        _heightMap[result.X, result.Y] = (a + b + c + d) / 4 + randomGenerator.NextFloat(min, max);
    }

    /// <summary>
    /// Method calculate square points, when method is called heightMap already must be initialized.
    /// Also method must be called after call of Diamond method.
    /// Calculate square points from 3 neighboring non zero points.
    /// </summary>
    /// <param name="sideCenterX"></param>
    /// <param name="sideCenterY"></param>
    /// <param name="middle"></param>
    /// <param name="randomGenerator"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Square(Point first, Point second, Point third, Point result, IRandomGenerator randomGenerator, float min, float max)
    {
        if (_flags[result.X, result.Y])
        {
            return;
        }
        else
        {
            _flags[result.X, result.Y] = true;
        }

        float a = _heightMap[first.X, first.Y];
        float b = _heightMap[second.X, second.Y];
        float c = _heightMap[third.X, third.Y];

        _heightMap[result.X, result.Y] = (a + b + c) / 3 + randomGenerator.NextFloat(min, max);
    }

    /// <summary>
    /// Method calls another methods of algorithm.
    /// </summary>
    /// <param name="leftX"></param>
    /// <param name="leftY"></param>
    /// <param name="rightX"></param>
    /// <param name="rightY"></param>
    /// <param name="randomGenerator"></param>
    private void DiamondSquare(Point leftTop, Point rightBottom, IRandomGenerator randomGenerator, float min, float max)
    {
        var middle = (rightBottom.X - leftTop.X) / 2;
        var center = new Point(leftTop.X + middle, leftTop.Y + middle);

        var rightTop = new Point(rightBottom.X, leftTop.Y);
        var leftBottom = new Point(leftTop.X, rightBottom.Y);

        Diamond(leftTop, rightBottom, center, randomGenerator, min, max);

        var topCenter = new Point(center.X, leftTop.Y);
        var rightCenter = new Point(rightTop.X, center.Y);
        var bottomCenter = new Point(center.X, rightBottom.Y);
        var leftCenter = new Point(leftTop.X, center.Y);

        Square(leftTop, center, rightTop, topCenter, randomGenerator, min, max);
        Square(rightTop, center, rightBottom, rightCenter, randomGenerator, min, max);
        Square(rightBottom, center, leftBottom, bottomCenter, randomGenerator, min, max);
        Square(leftBottom, center, leftTop, leftCenter, randomGenerator, min, max);

        if (center.X - leftTop.X <= 1)
        {
            return;
        }

        var newMin = min * _attenuationCoefficient;
        var newMax = max * _attenuationCoefficient;

        DiamondSquare(leftTop, center, randomGenerator, newMin, newMax);
        DiamondSquare(topCenter, rightCenter, randomGenerator, newMin, newMax);
        DiamondSquare(leftCenter, bottomCenter, randomGenerator, newMin, newMax);
        DiamondSquare(center, rightBottom, randomGenerator, newMin, newMax);
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
        _heightMap = new float[size, size];
        _flags = new bool[size, size];

        _heightMap[0, 0] = randomGenerator.NextFloat(min, max);
        _heightMap[0, size - 1] = randomGenerator.NextFloat(min, max);
        _heightMap[size - 1, size - 1] = randomGenerator.NextFloat(min, max);
        _heightMap[size - 1, 0] = randomGenerator.NextFloat(min, max);

        _flags[0, 0] = true;
        _flags[0, size - 1] = true;
        _flags[size - 1, size - 1] = true;
        _flags[size - 1, 0] = true;

        DiamondSquare(new Point(0, 0), new Point(size - 1, size - 1), randomGenerator, seedMin, seedMax);

        _normalizator.Normalize(_heightMap);

        return _heightMap;
    }
}