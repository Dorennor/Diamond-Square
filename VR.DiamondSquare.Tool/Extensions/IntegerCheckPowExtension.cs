namespace VR.DiamondSquare.Tools.Extensions;

public static class IntegerCheckPowExtension
{
    public static bool IsPowerOfTwo(this int x) => (x != 0) && ((x & (x - 1)) == 0);
}