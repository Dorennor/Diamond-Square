using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VR.DiamondSquare.ViewModel.Abstractions;

public abstract class BasicViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}