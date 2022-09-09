﻿using System.Drawing;
using System.Text.RegularExpressions;
using VR.DiamondSquare.Model.Interfaces;
using VR.DiamondSquare.Model.Models;
using VR.DiamondSquare.Model.Services;
using VR.DiamondSquare.ViewModel.Abstractions;
using VR.DiamondSquare.ViewModel.Utils;

namespace VR.DiamondSquare.ViewModel.ViewModels;

public class MainViewModel : BasicViewModel
{
    /// <summary>
    /// Regex that filtered only range strings, for example "1.2 ; 10.5" or "1, 19".
    /// </summary>
    public static readonly Regex FloatRangeRegex = new Regex(@"^(?'min'[0-9]{1,6}(\.[0-9]{1,3})?)[ ]?\p{P}{1}[ ]?(?'max'[0-9]{1,6}(\.[0-9]{1,3})?)$");

    private static readonly Color[] _colors = new Color[256];

    private Bitmap _bitmapImage;
    private AlternativeNormalMap _alternativeNormalMap;
    private NormalMap _normalMap;

    private string _range;
    private int? _seed;
    private bool _isAlternativePalette;
    private float _min;
    private float _max;
    private int _size;
    private float[,] _heightMap;

    private IRandomGenerator _defaultRandomGenerator;
    private IHeightMapGenerator _heightMapper;
    private INormalMapGenerator _normalMapper;

    private RelayCommand _generateHeightMapCommand;
    private RelayCommand _generateNormalMapCommand;
    private RelayCommand _cleanImageCommand;

    static MainViewModel()
    {
        for (int i = 0; i < 256; i++)
        {
            _colors[i] = Color.FromArgb(255, i, i, i);
        }
    }

    public MainViewModel(IRandomGenerator defaultRandomGenerator, IHeightMapGenerator heightMapper, INormalMapGenerator normalMapper)
    {
        _defaultRandomGenerator = defaultRandomGenerator;
        _heightMapper = heightMapper;
        _normalMapper = normalMapper;

        Size = 1025;
        _min = 1;
        _max = 10;
        Range = $"{_min}; {_max}";
    }

    public RelayCommand GenerateHeightMapCommand
    {
        get
        {
            return _generateHeightMapCommand ?? new RelayCommand(obj =>
            {
                if (Seed.HasValue)
                {
                    IRandomGenerator randomGenerator = new RandomGenerator(Seed.Value);
                    _heightMap = _heightMapper.GenerateHeightMap(randomGenerator, _size, _min, _max);
                }
                else
                {
                    _heightMap = _heightMapper.GenerateHeightMap(_defaultRandomGenerator, _size, _min, _max);
                }

                BitmapImage = DrawBitmap(_size, (i, j) => _colors[(int)Math.Round(_heightMap[i, j])]);
            }, obj => _heightMap == null && _max > _min);
        }
    }

    public RelayCommand GenerateNormalMapCommand
    {
        get
        {
            return _generateNormalMapCommand ?? new RelayCommand(obj =>
            {
                if (IsAlternativePalette)
                {
                    _alternativeNormalMap = _normalMapper.GenerateAlternativeNormalMap(_heightMap);

                    BitmapImage = DrawBitmap(_size, (i, j) => Color.FromArgb(255, (int)Math.Round(_alternativeNormalMap.XVector[i, j]), (int)Math.Round(_alternativeNormalMap.YVector[i, j]), (int)Math.Round(_alternativeNormalMap.ZVector[i, j])));
                }
                else
                {
                    _normalMap = _normalMapper.GenerateNormalMap(_heightMap);

                    BitmapImage = DrawBitmap(_size, (i, j) => Color.FromArgb(255, (int)Math.Round(_normalMap.XVector[i, j]), (int)Math.Round(_normalMap.YVector[i, j]), 255));
                }
            }, obj => _heightMap != null);
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

                if (_alternativeNormalMap != null)
                {
                    _alternativeNormalMap = null;
                }
                if (_normalMap != null)
                {
                    _normalMap = null;
                }
            }, obj => _bitmapImage != null);
        }
    }

    public Bitmap BitmapImage
    {
        get => _bitmapImage;
        set
        {
            if (value == _bitmapImage)
            {
                return;
            }

            _bitmapImage?.Dispose();
            _bitmapImage = value;
            OnPropertyChanged();
        }
    }

    public int? Seed
    {
        get => _seed;
        set
        {
            if (value == null || value == _seed)
            {
                return;
            }

            _seed = value;
            OnPropertyChanged();
        }
    }

    public bool IsAlternativePalette
    {
        get => _isAlternativePalette;
        set
        {
            _isAlternativePalette = value;
            OnPropertyChanged();
        }
    }

    public string Range
    {
        get => _range;
        set
        {
            if (value == string.Empty || value == _range)
            {
                return;
            }

            _range = value;

            Match match = FloatRangeRegex.Match(_range);

            _min = Convert.ToSingle(match.Groups["min"].Value);
            _max = Convert.ToSingle(match.Groups["max"].Value);

            OnPropertyChanged();
        }
    }

    public int Size
    {
        get => _size;
        set
        {
            if (value == 0 || value == _size)
            {
                return;
            }

            _size = value;
            OnPropertyChanged();
        }
    }

    private static Bitmap DrawBitmap(int size, Func<int, int, Color> provider)
    {
        var bitmap = new Bitmap(size, size);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                bitmap.SetPixel(i, j, provider(i, j));
            }
        }

        return bitmap;
    }
}