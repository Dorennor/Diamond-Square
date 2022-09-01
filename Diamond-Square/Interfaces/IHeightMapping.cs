namespace Diamond_Square.Interfaces
{
    public interface IHeightMapping
    {
        float[,] GenerateHeightMap(IRandomGenerator randomGenerator);
    }
}