using Diamond_Square.Interfaces;

namespace Diamond_Square.Models
{
    public class RandomSeed : IRandomSeed
    {
        private readonly IRandomGenerator _randomGenerator;

        public RandomSeed(IRandomGenerator randomGenerator)
        {
            _randomGenerator = randomGenerator;
        }

        public float GetRandomSeed(float point, float coefficient, float roughness, float size)
        {
            return _randomGenerator.NextFloat(-point * coefficient * roughness / size, point * coefficient * roughness / size);
        }

        public float GetRandomSeed(float min, float max)
        {
            return _randomGenerator.NextFloat(min, max);
        }
    }
}