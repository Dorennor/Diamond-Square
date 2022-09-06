using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using VR.DiamondSquare.View.Core;

namespace VR.DiamondSquare.View.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private BitmapSource _imageSource;
        private Bitmap _bitmap;
        private int? _seed;
        private bool _isGreyPalette;

        private static IRandomGenerator _randomGenerator;
        private IHeightGenerator _heightMapping;
        private INormalMapper _normalMapping;

        private RelayCommand _heightMapCommand;
        private RelayCommand _normalMapCommand;
        private RelayCommand _cleanImageCommand;

        public MainViewModel()
        {
            _randomGenerator = new RandomGenerator();
            _heightMapping = new HeightGenerator();
            _normalMapping = new NormalMapper();

            Bitmap bitmap = new Bitmap(HeightGenerator.Size, HeightGenerator.Size);

            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);

            _bitmap = bitmap;
        }

        public RelayCommand HeightMapCommand
        {
            get
            {
                return _heightMapCommand ?? new RelayCommand(obj =>
                {
                    if (Seed != null)
                    {
                        _randomGenerator = new RandomGenerator(Seed);
                    }

                    _bitmap = _heightMapping.GenerateHeightMap(_randomGenerator);
                    ImageSource = _bitmap.GetImageSource();
                });
            }
        }

        public RelayCommand NormalMapCommand
        {
            get
            {
                return _normalMapCommand ?? new RelayCommand(obj =>
                {
                    _bitmap = _normalMapping.GenerateNormalMap(_bitmap, IsGreyPalette);
                    ImageSource = _bitmap.GetImageSource();
                });
            }
        }

        public RelayCommand CleanImageCommand
        {
            get
            {
                return _cleanImageCommand ?? new RelayCommand(obj =>
                {
                    Bitmap bitmap = new Bitmap(HeightGenerator.Size, HeightGenerator.Size);

                    Graphics graphics = Graphics.FromImage(bitmap);
                    graphics.Clear(Color.White);

                    _bitmap = bitmap;
                    ImageSource = _bitmap.GetImageSource();
                });
            }
        }

        public BitmapSource ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                _imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        public int? Seed
        {
            get
            {
                return _seed;
            }
            set
            {
                _seed = Convert.ToInt32(value);
                OnPropertyChanged("Seed");
            }
        }

        public bool IsGreyPalette
        {
            get
            {
                return _isGreyPalette;
            }
            set
            {
                _isGreyPalette = value;
                OnPropertyChanged("IsGreyPalette");
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}