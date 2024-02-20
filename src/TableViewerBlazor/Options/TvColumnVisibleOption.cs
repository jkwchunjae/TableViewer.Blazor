namespace TableViewerBlazor.Options;

public class TvColumnVisibleOption
{
    public required IEnumerable<string> Before { get; set; }
    public required IEnumerable<string> After { get; set; }

    public bool Matched(IEnumerable<string> keys)
    {
        return Before.All(key => keys.Contains(key));
    }

    public bool Matched<T>(IEnumerable<T> keys, Func<T, string> getKey)
    {
        return Before.All(before => keys.Any(key => getKey(key) == before));
    }

    public IEnumerable<string> NewKeys(IEnumerable<string> keys)
    {
        return After.Where(key => keys.Contains(key));
    }

    public IEnumerable<T> NewKeys<T>(IEnumerable<T> keyInfos, Func<T, string> getKey)
    {
        return keyInfos.Where(keyInfo => After.Any(after => getKey(keyInfo) == after));
    }
}