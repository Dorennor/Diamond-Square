using System.Drawing;
using System.Text.RegularExpressions;
using VR.DiamondSquare.Model.Interfaces;
using VR.DiamondSquare.Model.Models;
using VR.DiamondSquare.ViewModel.Core;
using VR.DiamondSquare.ViewModel.Models;

namespace VR.DiamondSquare.ViewModel.ViewModels;

public class MainViewModel : BasicViewModel
{
    /// <summary>
    /// Regex that filtered only range strings, for example "1.2 ; 10.5" or "1, 19".
    /// </summary>
    private Regex regex = new Regex(@"^(?'min'[0-9]{1,6}(\.[0-9]{1,3})?)[ ]?\p{P}{1}[ ]?(?'max'[0-9]{1,6}(\.[0-9]{1,3})?)$");

    private Color[] _colors = new Color[256];

    private Bitmap _bitmapImage;
    private GreyNormalMap _greyNormalMap;
    private NormalMap _normalMap;
    private string _range;
    private int? _seed;
    private bool _isGreyPalette;
    private float? _min;
    private float? _max;
    private int? _size;

    private float[,] _heightMap;

    private IRandomGenerator _defaultRandomGenerator;
    private IHeightMapGenerator _heightMapper;
    private INormalMapGenerator _normalMapper;

    private RelayCommand _generateHeightMapCommand;
    private RelayCommand _generatNormalMapCommand;
    private RelayCommand _cleanImageCommand;

    public MainViewModel(IRandomGenerator defaultRandomGenerator, IHeightMapGenerator heightMapper, INormalMapGenerator normalMapper)
    {
        _defaultRandomGenerator = defaultRandomGenerator;
        _heightMapper = heightMapper;
        _normalMapper = normalMapper;

        for (int i = 0; i < 256; i++)
        {
            _colors[i] = Color.FromArgb(255, i, i, i);
        }
    }

    public RelayCommand GenerateHeightMapCommand
    {
        get
        {
            return _generateHeightMapCommand ?? new RelayCommand(obj =>
            {
                if (Seed.HasValue)
                {
                    IRandomGenerator randomGenerator = new RandomGenerator((int)Seed);
                    _heightMap = _heightMapper.GenerateHeightMap(randomGenerator, (int)_size, (float)_min, (float)_max);

                    using Bitmap bitmap = new Bitmap((int)_size, (int)_size);

                    for (int i = 0; i < _heightMap.GetLength(0); i++)
                    {
                        for (int j = 0; j < _heightMap.GetLength(1); j++)
                        {
                            bitmap.SetPixel(i, j, _colors[(int)Math.Round(_heightMap[i, j])]);
                        }
                    }

                    BitmapImage = bitmap;
                }
                else
                {
                    _heightMap = _heightMapper.GenerateHeightMap(_defaultRandomGenerator, (int)_size, (float)_min, (float)_max);

                    using Bitmap bitmap = new Bitmap((int)_size, (int)_size);

                    for (int i = 0; i < _heightMap.GetLength(0); i++)
                    {
                        for (int j = 0; j < _heightMap.GetLength(1); j++)
                        {
                            bitmap.SetPixel(i, j, _colors[(int)Math.Round(_heightMap[i, j])]);
                        }
                    }

                    BitmapImage = bitmap;
                }
            }, obj => _heightMap == null && _size.HasValue && _min.HasValue && _max.HasValue);
        }
    }

    public RelayCommand GeneratNormalMapCommand
    {
        get
        {
            return _generatNormalMapCommand ?? new RelayCommand(obj =>
            {
                if (IsGreyPalette)
                {
                    _greyNormalMap = _normalMapper.GenerateGreyNormalMap(_heightMap);

                    using Bitmap bitmap = new Bitmap((int)_size, (int)_size);

                    for (int i = 0; i < _heightMap.GetLength(0); i++)
                    {
                        for (int j = 0; j < _heightMap.GetLength(1); j++)
                        {
                            Color color = Color.FromArgb(255, (int)Math.Round(_greyNormalMap.XVector[i, j]), (int)Math.Round(_greyNormalMap.YVector[i, j]), (int)Math.Round(_greyNormalMap.ZVector[i, j]));
                            bitmap.SetPixel(i, j, color);
                        }
                    }

                    BitmapImage = bitmap;
                }
                else
                {
                    _normalMap = _normalMapper.GenerateNormalMap(_heightMap);

                    using Bitmap bitmap = new Bitmap((int)_size, (int)_size);

                    for (int i = 0; i < _heightMap.GetLength(0); i++)
                    {
                        for (int j = 0; j < _heightMap.GetLength(1); j++)
                        {
                            Color color = Color.FromArgb(255, (int)Math.Round(_normalMap.XVector[i, j]), (int)Math.Round(_normalMap.YVector[i, j]), 255);
                            bitmap.SetPixel(i, j, color);
                        }
                    }

                    BitmapImage = bitmap;
                }
            }, obj => _heightMap != null && _greyNormalMap == null && _normalMap == null && _size.HasValue && _min.HasValue && _max.HasValue);
        }
    }

    public RelayCommand CleanImageCommand
    {
        get
        {
            return _cleanImageCommand ?? new RelayCommand(obj =>
            {
                BitmapImage = null;
                _heightMap = null;

                if (_greyNormalMap != null) _greyNormalMap = null;
                if (_normalMap != null) _normalMap = null;
            }, obj => _bitmapImage != null || _heightMap != null || _greyNormalMap != null || _normalMap != null);
        }
    }

    public Bitmap BitmapImage
    {
        get => _bitmapImage;
        set
        {
            _bitmapImage = value;
            OnPropertyChanged();
        }
    }

    public int? Seed
    {
        get => _seed;
        set
        {
            if (value == null) return;

            _seed = value;
            OnPropertyChanged();
        }
    }

    public bool IsGreyPalette
    {
        get => _isGreyPalette;
        set
        {
            _isGreyPalette = value;
            OnPropertyChanged();
        }
    }

    public string Range
    {
        get => _range;
        set
        {
            if (value == string.Empty) return;

            _range = value;

            Match match = regex.Match(_range);

            _min = Convert.ToSingle(match.Groups["min"].Value);
            _max = Convert.ToSingle(match.Groups["max"].Value);

            OnPropertyChanged();
        }
    }

    public int? Size
    {
        get => _size;
        set
        {
            if (value == null) return;

            _size = value;
            OnPropertyChanged();
        }
    }
}