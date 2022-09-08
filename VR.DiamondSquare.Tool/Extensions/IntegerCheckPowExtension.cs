namespace VR.DiamondSquare.Tools.Extensions;

public static class IntegerCheckIsPowerOfTwoExtension
{
    public static bool IsPowerOfTwo(this int x) => (x != 0) && ((x & (x - 1)) == 0);
}