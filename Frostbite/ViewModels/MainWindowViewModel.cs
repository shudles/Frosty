using Frostbite.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Frostbite.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private readonly IUndoRedoService _undoRedoService;

    public MainWindowViewModel()
    {
        _undoRedoService = new UndoRedoService();

        _undoRedoService.UndoRedoTriggered += (sender, args) =>
        {
            OnPropertyChanged(nameof(CanUndo));
            OnPropertyChanged(nameof(CanRedo));
        };

        Shapes = new ObservableCollection<CircleViewModel>();

        Spawn = new RelayCommand<object>((_) =>
        {
            var items = Shapes.ToArray();
            _undoRedoService.Do(new CreateCircle(Shapes.Add, Shapes.Remove));
        });

        Undo = new RelayCommand<object>((_) =>
        {
            _undoRedoService.Undo();
        });

        Redo = new RelayCommand<object>((_) =>
        {
            _undoRedoService.Redo();
        });

        var fromX = 0d;
        var fromY = 0d;

        OnGrabCommand = new RelayCommand<CircleViewModel>(c =>
        {
            fromX = c!.Left;
            fromY = c.Top;
            // todo maybe make relay command always check param is not null?
        }, c => c is not null);

        OnReleaseCommand = new RelayCommand<CircleViewModel>(c =>
        {
            _undoRedoService.Do(new MoveCircle(c, fromX, fromY, c.Left, c.Top));
        });
    }

    public bool CanUndo => _undoRedoService.UndoCount > 0;
    public bool CanRedo => _undoRedoService.RedoCount > 0;

    public ICommand Spawn { get; }

    public ICommand Undo { get; }

    public ICommand Redo { get; }

    public ICommand OnGrabCommand { get; }

    public ICommand OnReleaseCommand { get; }

    public ObservableCollection<CircleViewModel> Shapes { get; set; }

    public override void Dispose()
    {
        base.Dispose();
        // todo do this properly with func reference
        _undoRedoService.UndoRedoTriggered -= (sender, args) => OnPropertyChanged(nameof(CanUndo));
    }
}

public record MoveAction
{
    public double XFrom {get; set;}
    public double YFrom {get; set;}
    public double YTo {get; set;}
    public double XTo {get; set;}
}
