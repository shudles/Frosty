using System;
using System.Windows.Input;

namespace Frostbite;

public class RelayCommand<T> : ICommand where T : class?
{
    readonly Action<T?> _execute;
    readonly Predicate<T?> _canExecute;

    public RelayCommand(Action<T?> execute) : this(execute, _ => true)
    {
    }

    public RelayCommand(Action<T?> execute, Predicate<T?> canExecute)
    {
        // todo library for null references?
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
    }
    public bool CanExecute(object? parameter) => parameter switch
    {
        not T and not null => throw new ArgumentException($"{nameof(parameter)} should be of type T, it was {parameter?.GetType()}"),
        T t => _canExecute(t),
        _ => _canExecute(null)
    };

    public void Execute(object? parameter)
    {
        switch (parameter)
        {
            case not T and not null:
                throw new ArgumentException($"{nameof(parameter)} should be of type T, it was {parameter?.GetType()}");
            case T t:
                _execute(t);
                break;
            default:
                _execute(null);
                break;
        }
    }

    public event EventHandler? CanExecuteChanged // todo understand this more
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}
