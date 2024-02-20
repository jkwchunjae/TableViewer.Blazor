namespace TableViewerBlazor.Options;

public class TvColumnVisibleOption
{
    public required IEnumerable<string> Before { get; set; }
    public required IEnumerable<string> After { get; set; }

    public bool Matched(IEnumerable<string> keys)
    {
        return Before.All(key => keys.Contains(key));
    }

    public IEnumerable<string> NewKeys(IEnumerable<string> keys)
    {
        return After.Where(key => keys.Contains(key));
    }
}