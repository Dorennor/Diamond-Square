using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VR.DiamondSquare.ViewModel.Abstractions;

public abstract class BasicViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    protected readonly Dictionary<string, HashSet<string>> ErrorsByPropertyName = new Dictionary<string, HashSet<string>>();

    public event PropertyChangedEventHandler? PropertyChanged;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public bool HasErrors => ErrorsByPropertyName.Any();

    public IEnumerable GetErrors(string propertyName)
    {
        if (!ErrorsByPropertyName.TryGetValue(propertyName, out HashSet<string> result))
        {
            result = null;
        }
        return result;
    }

    protected void AddError(string propertyName, string error)
    {
        if (!ErrorsByPropertyName.TryGetValue(propertyName, out HashSet<string> collection))
        {
            collection = new HashSet<string>();

            ErrorsByPropertyName.Add(propertyName, collection);
        }

        if (collection.Add(error))
        {
            OnErrorsChanged(propertyName);
        }
    }

    protected void CleanErrors(string propertyName)
    {
        if (ErrorsByPropertyName.Remove(propertyName))
        {
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