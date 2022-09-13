using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VR.DiamondSquare.ViewModel.Abstractions;

public abstract class BasicViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    protected readonly Dictionary<string, List<string>> ErrorsByPropertyName = new Dictionary<string, List<string>>();

    public event PropertyChangedEventHandler? PropertyChanged;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public bool HasErrors => ErrorsByPropertyName.Any();

    public IEnumerable GetErrors(string propertyName)
    {
        return ErrorsByPropertyName.ContainsKey(propertyName) ? ErrorsByPropertyName[propertyName] : null;
    }

    protected void AddError(string propertyName, string error)
    {
        if (!ErrorsByPropertyName.ContainsKey(propertyName))
            ErrorsByPropertyName[propertyName] = new List<string>();

        if (!ErrorsByPropertyName[propertyName].Contains(error))
        {
            ErrorsByPropertyName[propertyName].Add(error);
            OnErrorsChanged(propertyName);
        }
    }

    protected void CleanErrors(string propertyName)
    {
        if (ErrorsByPropertyName.ContainsKey(propertyName))
        {
            ErrorsByPropertyName.Remove(propertyName);
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