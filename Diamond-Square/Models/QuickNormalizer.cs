using System;
using Diamond_Square.Interfaces;

namespace Diamond_Square.Models
{
    public class QuickNormalizer : INormalization
    {
        private readonly int _min;
        private readonly int _max;

        public QuickNormalizer(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public int[,] Normalize(float[,] heightMap)
        {
            int[,] resultHeightMap = new int[heightMap.GetLength(0), heightMap.GetLength(1)];

            for (int i = 0; i < heightMap.GetLength(0); i++)
            {
                for (int j = 0; j < heightMap.GetLength(1); j++)
                {
                    resultHeightMap[i, j] = (int)Math.Round(heightMap[i, j] * 255);

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
}