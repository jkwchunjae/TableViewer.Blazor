namespace TableViewerBlazor.Options;

public abstract class TvFieldAttribute : Attribute
{
    public string Id { get; init; }
    public TvFieldAttribute(string id)
    {
        Id = id;
    }
}
