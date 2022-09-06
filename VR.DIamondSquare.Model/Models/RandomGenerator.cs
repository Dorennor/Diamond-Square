using VR.DiamondSquare.View.Interfaces;

namespace VR.DiamondSquare.View.Models
{
    public class RandomGenerator : IRandomGenerator
    {
        private readonly Random random;

        public RandomGenerator()
        {
            random = new Random();
        }

        public RandomGenerator(int? seed)
        {
            random = new Random((int)seed);
        }

        public float NextFloat(float min, float max)
        {
            return random.NextFloat(min, max);
        }
    }
}