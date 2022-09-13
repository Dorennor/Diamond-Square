using VR.DiamondSquare.Model.Interfaces;

namespace VR.DiamondSquare.Model.Services;

/// <summary>
/// Container for random generator, need to use custom random generators in future.
/// </summary>
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

    /// <summary>
    /// Return random float number in range, method uses double random generator in math formula
    /// to transform 0-1 result of double generator to result in min-max range.
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public float NextFloat(float min, float max)
    {
        return min + (float)_random.NextDouble() * (max - min);
    }
}