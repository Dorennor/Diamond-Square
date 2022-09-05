using System;
using Diamond_Square.Extensions;
using Diamond_Square.Interfaces;

namespace Diamond_Square.Models
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