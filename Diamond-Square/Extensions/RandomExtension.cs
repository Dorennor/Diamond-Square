using System;

namespace Diamond_Square.Extensions
{
    internal static class RandomDoubleFromRangeExtension
    {
        internal static float RandRange(this Random r, float rMin, float rMax)
        {
            return rMin + (float)r.NextDouble() * (rMax - rMin);
        }
    }
}