using Diamond_Square.UI.Core;

namespace Diamond_Square.UI.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        public RelayCommand PlotViewModelCommand { get; set; }
        public RelayCommand NormalMapViewModelCommand { get; set; }

        public PlotViewModel PlotViewModel { get; set; }
        public NormalMapViewModel NormalMapViewModel { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            PlotViewModel = new PlotViewModel();
            NormalMapViewModel = new NormalMapViewModel();

            CurrentView = PlotViewModel;

            PlotViewModelCommand = new RelayCommand(command =>
            {
                CurrentView = PlotViewModel;
            });

            NormalMapViewModelCommand = new RelayCommand(command =>
            {
                CurrentView = NormalMapViewModel;
            });
        }
    }
}