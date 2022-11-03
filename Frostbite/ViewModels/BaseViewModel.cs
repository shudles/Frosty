using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Frostbite.ViewModels;

public class BaseViewModel : INotifyPropertyChanged, IDisposable
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public virtual void Dispose()
    {
    }
}
