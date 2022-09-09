namespace VR.DiamondSquare.Model.Models;

public class AlternativeNormalMap : NormalMap
{
    public float[,] ZVector { get; set; }

    public AlternativeNormalMap(int size) : base(size)
    {
        ZVector = new float[size, size];
    }
}