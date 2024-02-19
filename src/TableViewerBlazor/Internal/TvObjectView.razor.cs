using System.Collections;
using Microsoft.AspNetCore.Components;

namespace TableViewerBlazor.Internal;

public partial class TvObjectView : TvViewBase
{
    [Parameter] public object? Data { get; set; }

    bool Open = false;

    private IEnumerable<(object Key, object? Value)> Items =>
        Data switch
        {
            IDictionary dictionary => Convert(dictionary),
            _ => Convert(Data!),
        };


    protected override void OnInitialized()
    {
        if (Options != null)
        {
            Open = Depth <= Options.OpenDepth;
        }
    }

    private IEnumerable<(object Key, object? Value)> Convert(IDictionary dictionary)
    {
        foreach (var key in dictionary.Keys)
        {
            yield return (key, dictionary[key]);
        }
    }

    private IEnumerable<(object Key, object? Value)> Convert(object obj)
    {
        if (Options?.ReadProperty ?? false)
        {
            var properties = obj.GetType().GetProperties()
                .Where(p => p.CanRead)
                .Where(p => p.PropertyType != typeof(Type));
            foreach (var property in properties)
            {
                yield return (property.Name, property.GetValue(obj));
            }
        }

        if (Options?.ReadField ?? false)
        {
            var fields = obj.GetType().GetFields()
                .Where(f => f.IsPublic)
                .Where(f => f.FieldType != typeof(Type));
            foreach (var field in fields)
            {
                yield return (field.Name, field.GetValue(obj));
            }
        }
    }

    private void ToggleOpen()
    {
        Open = !Open;
    }
}
