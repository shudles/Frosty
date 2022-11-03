namespace Frostbite.Tests.Commands;

public class RelayCommandTests
{
    [Fact]
    public void AllowsNullParameter()
    {
        // setup
        var command = new RelayCommand<RelayCommandTests>(_ => { }, _ => true);

        // act
        command.CanExecute(null);
        command.Execute(null);

        // assert
    }

    [Fact]
    public void AllowsRejectsWrongType()
    {
        // setup
        var command = new RelayCommand<RelayCommandTests>(_ => { }, _ => true);

        // act & assert
        Assert.Throws<ArgumentException>(() => command.CanExecute(new object()));
        Assert.Throws<ArgumentException>(() => command.Execute(new object()));
    }
}
