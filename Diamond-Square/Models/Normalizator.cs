using System;
using System.Linq;
using Diamond_Square.Interfaces;

namespace Diamond_Square.Models
{
    public class Normalizator : INormalization
    {
        private readonly int _min;
        private readonly int _max;

        public Normalizator(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public int[,] Normalize(float[,] heightMap)
        {
            int[,] resultHeightMap = new int[heightMap.GetLength(0), heightMap.GetLength(1)];

            float minValue = heightMap.Cast<float>().Min();
            float maxValue = heightMap.Cast<float>().Max(); ;

            for (int i = 0; i < heightMap.GetLength(0); i++)
            {
                for (int j = 0; j < heightMap.GetLength(1); j++)
                {
                    resultHeightMap[i, j] = (int)Math.Round(((heightMap[i, j] - minValue) / (maxValue - minValue)) * 255);
                }
            }

            return resultHeightMap;
        }
    }
}