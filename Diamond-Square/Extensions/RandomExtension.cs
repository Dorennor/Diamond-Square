using System;

namespace Diamond_Square.Extensions
{
    internal static class RandomDoubleFromRangeExtension
    {
        internal static float NextFloat(this Random random, float min, float max)
        {
            return min + (float)random.NextDouble() * (max - min);
        }
    }
}