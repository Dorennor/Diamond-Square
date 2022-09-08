namespace VR.DiamondSquare.Model.Models;

public class GreyNormalMap : NormalMap
{
    public float[,] ZVector { get; set; }

    public GreyNormalMap(int size) : base(size)
    {
        ZVector = new float[size, size];
    }
}