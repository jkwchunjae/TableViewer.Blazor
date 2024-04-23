namespace TableViewerBlazor.Options;

public interface ITeFieldOption
{
    string? Id { get; }
    Func<object?, int, string, bool>? Condition { get; }
}


public class TeFieldOption<T> : ITeFieldOption
{
    public string? Id { get; set; }
    public Func<T?, int, string, bool>? Condition { get; set; }

    Func<object?, int, string, bool>? ITeFieldOption.Condition =>
        (value, index, path) =>
        {
            if (value is T tValue)
            {
                return Condition?.Invoke(tValue, index, path) ?? true;
            }
            else
            {
                return false;
            }
        };

}