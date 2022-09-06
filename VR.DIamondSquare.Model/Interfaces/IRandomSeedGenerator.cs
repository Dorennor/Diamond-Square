namespace VR.DiamondSquare.View.Interfaces
{
    public interface IRandomSeedGenerator
    {
        float GetRandomSeed(float point, float coefficient, float roughness, float size);

        float GetRandomSeed(float min, float max);
    }
}