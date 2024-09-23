namespace TableViewerBlazor.Options;

public interface ITvFieldOption
{
    string? Id { get; }
}

public abstract class TvFieldAttribute : Attribute
{
    public string Id { get; init; }
    public TvFieldAttribute(string id)
    {
        Id = id;
    }
}
