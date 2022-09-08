using VR.DiamondSquare.Model.Models;

namespace VR.DiamondSquare.Model.Interfaces;

public interface INormalMapGenerator
{
    NormalMap GenerateNormalMap(float[,] heightMap);

    GreyNormalMap GenerateGreyNormalMap(float[,] heightMap);
}