namespace TableViewerBlazor.Public;

public class TvStringAttribute<T> : Attribute
{
    private TvStringConverter<T> converter;

    public TvStringAttribute(TvStringConverter<T> converter)
    {
        this.converter = converter;
    }
}

public class  TvStringConverter<T>
{
    public required Func<T, string> ToStringFunc { get; init; }
    public required Func<string, T> ToObjectFunc { get; init; }
}
