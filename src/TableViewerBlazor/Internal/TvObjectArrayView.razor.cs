using Microsoft.AspNetCore.Components;

namespace TableViewerBlazor.Internal;

public partial class TvObjectArrayView : TvViewBase
{
    [Parameter] public IEnumerable<object?> Data { get; set; } = null!;

    IEnumerable<object?> array => Data ?? Enumerable.Empty<object?>();

    private string[] Keys = Array.Empty<string>();

    protected override Task OnInitializedAsync()
    {
        var firstData = Data.FirstOrDefault(x => x != null);
        if (firstData != null)
        {
            Keys = firstData.GetKeys().ToArray();
        }
        return base.OnInitializedAsync();
    }

    private object? GetValue(object? item, string key)
    {
        var dataType = item?.GetType();
        var property = dataType?.GetProperty(key);
        if (property != null)
        {
            return property.GetValue(item);
        }
        var field = dataType?.GetField(key);
        if (field != null)
        {
            return field.GetValue(item);
        }
        return null;
    }
}

public static class ObjectArrayHelper
{
    public static IEnumerable<string> GetKeys(this object? data)
    {
        if (data == null)
            return Enumerable.Empty<string>();

        var dataType = data.GetType();
        var properties = dataType.GetProperties();
        var fields = dataType.GetFields();

        return properties.Select(p => p.Name)
            .Concat(fields.Select(f => f.Name));
    }
}