namespace Frostbite.ViewModels;

public class CircleViewModel : BaseViewModel
{
    public int Radius { get; set; }

    private double _top;
    public double Top
    {
        get => _top;
        set
        {
            _top = value;
            OnPropertyChanged();
        }
    }

    private double _left;
    public double Left
    {
        get => _left;
        set
        {
            _left = value;
            OnPropertyChanged();
        }
    }
}
