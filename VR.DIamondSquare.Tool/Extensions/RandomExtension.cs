namespace VR.DIamondSquare.Tools.Extensions
{
    public static class RandomDoubleFromRangeExtension
    {
        public static float NextFloat(this Random random, float min, float max)
        {
            return min + (float)random.NextDouble() * (max - min);
        }
    }
}