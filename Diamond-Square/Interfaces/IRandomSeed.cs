namespace Diamond_Square.Interfaces
{
    public interface IRandomSeed
    {
        float GetRandomSeed(float point, float coefficient, float roughness, float size);

        float GetRandomSeed(float min, float max);
    }
}