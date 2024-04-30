namespace TableViewerBlazor.Options;

public interface ITeValidation
{
    Func<object, Task<bool>> Func { get; }
    string Message { get; }
}

public class TeValidation<T> : ITeValidation
{
    public required Func<T, bool> Func { get; set; }
    public required string Message { get; set; }
    Func<object, Task<bool>> ITeValidation.Func =>
        value =>
        {
            if (value is T tValue)
            {
                var result = Func?.Invoke(tValue) ?? true;
                return Task.FromResult(result);
            }
            else
            {
                return Task.FromResult(false);
            }
        };
}

public class TeAsyncValidation<T> : ITeValidation
{
    public required Func<T, Task<bool>> Func { get; set; }

    public required string Message { get; set; }

    Func<object, Task<bool>> ITeValidation.Func =>
        async value =>
        {
            if (value is T tValue)
            {
                if (Func != null)
                {
                    return await Func.Invoke(tValue);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        };
}
