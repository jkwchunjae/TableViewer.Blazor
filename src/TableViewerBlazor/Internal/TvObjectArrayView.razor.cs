using Microsoft.AspNetCore.Components;

namespace TableViewerBlazor.Internal;

public partial class TvObjectArrayView : TvViewBase
{
    [Parameter] public IEnumerable<object?> Data { get; set; } = null!;

    bool Open = false;

    IEnumerable<object?> array => Data ?? Enumerable.Empty<object?>();

    private string[] Keys = Array.Empty<string>();
    private int TableColumns => Keys.Length + 1;

    protected override void OnInitialized()
    {
        var firstData = Data.FirstOrDefault(x => x != null);
        if (firstData != null)
        {
            Keys = firstData.GetKeys().ToArray();
        }
        if (Options != null)
        {
            Open = Depth <= Options.OpenDepth;
        }
    }

    private object? GetValue(object? item, string key)
    {
        var dataType = item?.GetType();
        var property = dataType?.GetProperty(key);
        if (property != null)
        {
            return property.GetValue(item);
        }
        //var field = dataType?.GetField(key);
        //if (field != null)
        //{
        //    return field.GetValue(item);
        //}
        return null;
    }

    private void ToggleOpen()
    {
        Open = !Open;
    }
}

public static class ObjectArrayHelper
{
    public static IEnumerable<string> GetKeys(this object? data)
    {
        if (data == null)
            yield break;

        var dataType = data.GetType();

        var properties = dataType.GetProperties()
            .Where(p => p.Name != "Parser")
            .Where(p => p.Name != "Descriptor")
            ;
        foreach (var property in properties)
        {
            yield return property.Name;
        }

        //var fields = dataType.GetFields();
        //foreach (var field in fields)
        //{
        //    yield return field.Name;
        //}
    }
}