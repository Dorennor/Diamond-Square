using VR.DiamondSquare.Model.Interfaces;

namespace VR.DiamondSquare.Model.Services
{
    public class RandomGenerator : IRandomGenerator
    {
        private readonly Random _random;

        public RandomGenerator()
        {
            _random = new Random();
        }

        public RandomGenerator(int seed)
        {
            _random = new Random(seed);
        }

        public float NextFloat(float min, float max)
        {
            return min + (float)_random.NextDouble() * (max - min);
        }
    }
}