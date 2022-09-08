namespace VR.DiamondSquare.Model.Models;

public class NormalMap
{
    public float[,] XVector { get; set; }
    public float[,] YVector { get; set; }
    public int Length { get; private set; }

    public NormalMap(int size)
    {
        XVector = new float[size, size];
        YVector = new float[size, size];
        Length = size;
    }
}