using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VR.DiamondSquare.ViewModel.Abstractions;

public abstract class BasicViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    protected readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

    public event PropertyChangedEventHandler? PropertyChanged;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public bool HasErrors => _errorsByPropertyName.Any();

    public IEnumerable GetErrors(string propertyName)
    {
        return _errorsByPropertyName.ContainsKey(propertyName) ? _errorsByPropertyName[propertyName] : null;
    }

    protected void AddError(string propertyName, string error)
    {
        if (!_errorsByPropertyName.ContainsKey(propertyName))
            _errorsByPropertyName[propertyName] = new List<string>();

        if (!_errorsByPropertyName[propertyName].Contains(error))
        {
            _errorsByPropertyName[propertyName].Add(error);
            OnErrorsChanged(propertyName);
        }
    }

    protected void CleanErrors(string propertyName)
    {
        if (_errorsByPropertyName.ContainsKey(propertyName))
        {
            _errorsByPropertyName.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }
    }

    protected void OnErrorsChanged([CallerMemberName] string? propertyName = null)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}