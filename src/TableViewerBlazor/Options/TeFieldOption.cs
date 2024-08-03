namespace TableViewerBlazor.Options;

public interface ITeFieldOption
{
    string? Id { get; }
    Func<object?, int, string, bool>? Condition { get; }
    ITeConverter Converter { get; }
}

public interface ITeFieldOption<TValue> : ITeFieldOption
{
    new Func<TValue?, int, string, bool>? Condition { get; }
    new ITeConverter<TValue> Converter { get; }

    Func<object?, int, string, bool>? ITeFieldOption.Condition =>
        (value, index, path) =>
        {
            if (value is TValue tValue)
            {
                return Condition?.Invoke(tValue, index, path) ?? true;
            }
            else
            {
                return false;
            }
        };
}

public interface ITeConverter
{
    Func<object, object> ToField { get; }
    Func<object, object> FromField { get; }
}

public interface ITeConverter<TValue> : ITeConverter
{
    new Func<TValue, object> ToField { get; }
    new Func<object, TValue> FromField { get; }
}