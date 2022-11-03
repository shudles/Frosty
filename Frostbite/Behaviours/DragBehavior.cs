using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows.Input;

namespace Frostbite.Behaviours;

public class DragBehavior : Behavior<UIElement>
{
    private Point elementStartPosition;
    private Point mouseStartPosition;
    private Canvas? canvas;
    private ContentPresenter? content;

    protected override void OnAttached()
    {
        var parent = Application.Current.MainWindow;
        content = AssociatedObject.GetSelfAndAncestors().First(t => t is ContentPresenter) as ContentPresenter;
        canvas = AssociatedObject.GetSelfAndAncestors().First(t => t is Canvas) as Canvas;


        AssociatedObject.MouseLeftButtonDown += (sender, e) =>
        {
            elementStartPosition = AssociatedObject.TranslatePoint(new(), parent);
            mouseStartPosition = e.GetPosition(parent);
            AssociatedObject.CaptureMouse();
        };

        AssociatedObject.MouseLeftButtonUp += (sender, e) =>
        {
            AssociatedObject.ReleaseMouseCapture();
        };
        AssociatedObject.MouseMove += (sender, e) =>
        {
            if (AssociatedObject.IsMouseCaptured)
            {
                var mouseMovePosition = Mouse.GetPosition(canvas);
                var currentPosition = new Point(Canvas.GetLeft(content), Canvas.GetTop(content));
                var diff = mouseMovePosition - mouseStartPosition;

                Canvas.SetLeft(content, elementStartPosition.X + diff.X);
                Canvas.SetTop(content, elementStartPosition.Y + diff.Y);
            }
        };
    }
}
