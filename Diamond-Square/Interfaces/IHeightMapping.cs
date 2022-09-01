namespace Diamond_Square.Interfaces
{
    public interface IHeightMapping
    {
        int[,] GenerateHeightMap(IRandomGenerator randomGenerator);
    }
}