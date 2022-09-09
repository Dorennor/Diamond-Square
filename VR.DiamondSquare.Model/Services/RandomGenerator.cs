using VR.DiamondSquare.Model.Interfaces;
using VR.DiamondSquare.Tools.Extensions;

namespace VR.DiamondSquare.Model.Services
{
    public class RandomGenerator : IRandomGenerator
    {
        private readonly Random random;

        public RandomGenerator()
        {
            random = new Random();
        }

        public RandomGenerator(int seed)
        {
            random = new Random(seed);
        }

        public float NextFloat(float min, float max)
        {
            return random.NextFloat(min, max);
        }
    }
}