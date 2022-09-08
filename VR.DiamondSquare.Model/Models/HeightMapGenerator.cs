using VR.DiamondSquare.Model.Interfaces;

namespace VR.DiamondSquare.Model.Models;

public class HeightMapGenerator : IHeightMapGenerator
{
    public int Size { get; private set; }

    private static readonly float _min = -1;
    private static readonly float _max = 1;

    private INormalizator _normalizator;
    private float[,] _heightMap;

    private void Diamond(int leftX, int leftY, int rightX, int rightY, IRandomGenerator randomGenerator)
    {
        int middle = (rightX - leftX) / 2;

        float a = _heightMap[leftX, leftY];
        float b = _heightMap[leftX, rightY];
        float c = _heightMap[rightX, rightY];
        float d = _heightMap[rightX, leftY];

        int centerX = leftX + middle;
        int centerY = leftY + middle;

        _heightMap[centerX, centerY] = (a + b + c + d) / 4 + randomGenerator.NextFloat(_min, _max);
    }

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

        _heightMap[sideCenterX, sideCenterY] = (a + b + c + d) / 4 + randomGenerator.NextFloat(_min, _max);
    }

    private void DiamondSquare(int leftX, int leftY, int rightX, int rightY, IRandomGenerator randomGenerator)
    {
        int middle = (rightX - leftX) / 2;

        Diamond(leftX, leftY, rightX, rightY, randomGenerator);

        Square(leftX, leftY + middle, middle, randomGenerator);
        Square(rightX, rightY - middle, middle, randomGenerator);
        Square(rightX - middle, rightY, middle, randomGenerator);
        Square(leftX + middle, leftY, middle, randomGenerator);
    }

    private void MiddlePointDisplacement(int middleX, int middleY, int rightX, int rightY, IRandomGenerator randomGenerator)
    {
        int middle = (rightX - middleX) / 2;

        if (middle > 0)
        {
            float a = _heightMap[middleX, middleY];
            float b = _heightMap[middleX, rightY];
            float c = _heightMap[rightX, rightY];
            float d = _heightMap[rightX, middleY];

            int centerX = middleX + middle;
            int centerY = middleY + middle;

            _heightMap[centerX, centerY] = (a + b + c + d) / 4 + randomGenerator.NextFloat(_min, _max);
            _heightMap[middleX, centerY] = (a + b) / 2 + randomGenerator.NextFloat(_min, _max);
            _heightMap[rightX, centerY] = (c + d) / 2 + randomGenerator.NextFloat(_min, _max);
            _heightMap[centerX, middleY] = (a + d) / 2 + randomGenerator.NextFloat(_min, _max);
            _heightMap[centerX, rightY] = (b + c) / 2 + randomGenerator.NextFloat(_min, _max);

            MiddlePointDisplacement(middleX, middleY, centerX, centerY, randomGenerator);
            MiddlePointDisplacement(middleX, middleY + middle, middleX + middle, rightY, randomGenerator);
            MiddlePointDisplacement(centerX, centerY, rightX, rightY, randomGenerator);
            MiddlePointDisplacement(middleX + middle, middleY, rightX, centerY, randomGenerator);
        }
    }

    public float[,] GenerateHeightMap(IRandomGenerator randomGenerator, int size, float min, float max)
    {
        Size = size;
        _heightMap = new float[Size, Size];

        _normalizator = new Normalizator(0, 255);

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
                }
            }
        }

        return _normalizator.Normalize(_heightMap);
    }
}