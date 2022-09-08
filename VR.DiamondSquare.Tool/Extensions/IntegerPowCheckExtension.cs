namespace VR.DiamondSquare.Tools.Extensions;

public static class IntegerPowCheckExtension
{
    public static bool IsPowerOfTwo(this int x)
    {
        return (x != 0) && ((x & (x - 1)) == 0);
    }
}