using Microsoft.AspNetCore.Components;
using TableViewerBlazor.Options;

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
            var keys = GetKeys(firstData);

            var columnOption = Options?.ColumnVisible?.FirstOrDefault(x => x.Matched(keys));
            if (columnOption != null)
            {
                keys = columnOption.NewKeys(keys).ToArray();
            }
            if (Options?.DisableKeys?.Any() ?? false)
            {
                keys = keys
                    .Where(key => Options!.DisableKeys!.All(disable => disable != key));
            }

            Keys = keys.ToArray();
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
        var field = dataType?.GetField(key);
        if (field != null)
        {
           return field.GetValue(item);
        }
        return null;
    }

    private void ToggleOpen()
    {
        Open = !Open;
    }

    private IEnumerable<string> GetKeys(object? data)
    {
        if (data == null)
            yield break;

        var dataType = data.GetType();

        if (Options?.ReadProperty ?? false)
        {
            var properties = dataType.GetProperties()
                .Where(p => p.CanRead)
                .Where(p => p.PropertyType != typeof(Type));
            foreach (var property in properties)
            {
                yield return property.Name;
            }
        }

        if (Options?.ReadField ?? false)
        {
            var fields = dataType.GetFields()
                .Where(f => f.IsPublic)
                .Where(f => f.FieldType != typeof(Type));
            foreach (var field in fields)
            {
                yield return field.Name;
            }
        }
    }

    private async Task ButtonAction(object? item, ITvAction action)
    {
        await action.Action(item);
    }
}