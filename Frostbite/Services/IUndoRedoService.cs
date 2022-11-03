using Frostbite.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;

namespace Frostbite.Services;

public interface IUnDoableAction
{
    public void Do();

    public void Undo();
}

public class CreateCircle : IUnDoableAction
{
    private readonly static Random _random = new();
    private readonly CircleViewModel _circleViewModel;
    private readonly Action<CircleViewModel> _add;
    private readonly Func<CircleViewModel, bool> _remove;

    public CreateCircle(Action<CircleViewModel> add, Func<CircleViewModel, bool> remove)
    {
        _circleViewModel = new() { Left = _random.Next(300), Top = _random.Next(300), Radius = _random.Next(30, 50) };
        _add = add;
        _remove = remove;
    }

    public void Do() => _add(_circleViewModel);
    public void Undo() => _remove(_circleViewModel);
}

public class MoveCircle : IUnDoableAction
{
    private readonly CircleViewModel _circle;
    private readonly double _fromX;
    private readonly double _fromY;
    private readonly double _toX;
    private readonly double _toY;

    public MoveCircle(CircleViewModel circle, double fromX, double fromY, double toX, double toY)
    {
        _circle = circle;
        _fromX = fromX;
        _fromY = fromY;
        _toX = toX;
        _toY = toY;
    }

    public void Do()
    {
        _circle.Top = _toY;
        _circle.Left = _toX;
    }

    public void Undo()
    {
        _circle.Top = _fromY;
        _circle.Left = _fromX;
    }
}

public interface IUndoRedoService
{
    int UndoCount { get; }
    int RedoCount { get; }
    void Do(IUnDoableAction action);

    void Undo();

    void Redo();

    event UndoRedoTriggered? UndoRedoTriggered; // TODO rename to ACtionHappened
}

public delegate void UndoRedoTriggered(object? sender, EventArgs e);

public class UndoRedoService : IUndoRedoService
{
    private Stack<IUnDoableAction> _undoStack;
    private Stack<IUnDoableAction> _redoStack;

    public UndoRedoService()
    {
        _undoStack = new();
        _redoStack = new();
    }

    public int UndoCount => _undoStack.Count;

    public int RedoCount => _redoStack.Count;

    public void Do(IUnDoableAction action)
    {
        action.Do();
        _undoStack.Push(action);
        UndoRedoTriggered?.Invoke(this, EventArgs.Empty);
    }

    public void Redo()
    {
        // TODO 
        var action = _redoStack.Pop();
        action.Do();
        _undoStack.Push(action);
        UndoRedoTriggered?.Invoke(this, EventArgs.Empty);
    }

    public void Undo()
    {
        var action = _undoStack.Pop();
        action.Undo();
        _redoStack.Push(action);
        UndoRedoTriggered?.Invoke(this, EventArgs.Empty);
    }

    public event UndoRedoTriggered? UndoRedoTriggered;
}
