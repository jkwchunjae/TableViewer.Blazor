namespace TableViewerBlazor.Options;

public interface ITeValidation
{
    Func<object, bool> Func { get; }
    string Message { get; }
}

public class TeValidation<T> : ITeValidation
{
    public required Func<T, bool> Func { get; set; }
    public required string Message { get; set; }
    Func<object, bool> ITeValidation.Func =>
        value =>
        {
            if (value is T tValue)
            {
                return Func?.Invoke(tValue) ?? false;
            }
            else
            {
                return false;
            }
        };
}