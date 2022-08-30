using System;

namespace Diamond_Square.Extensions
{
    internal static class RandomDoubleFromRangeExtension
    {
        internal static int RandRange(this Random r, int rMin, int rMax)
        {
            return rMin + r.Next() * (rMax - rMin);
        }

        internal static double RandRange(this Random r, double rMin, double rMax)
        {
            return rMin + r.NextDouble() * (rMax - rMin);
        }

        internal static float RandRange(this Random r, float rMin, float rMax)
        {
            return rMin + (float)r.NextDouble() * (rMax - rMin);
        }
    }
}