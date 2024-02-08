using System.Collections;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace TableViewerBlazor.Internal;

public partial class TvObjectView : TvViewBase
{
    [Parameter] public object? Data { get; set; }

    private IEnumerable<(object Key, object? Value)> Items =>
        Data switch
        {
            IDictionary dictionary => Convert(dictionary),
            _ => Convert(Data!),
        };

    private IEnumerable<(object Key, object? Value)> Convert(IDictionary dictionary)
    {
        foreach (var key in dictionary.Keys)
        {
            yield return (key, dictionary[key]);
        }
    }

    private IEnumerable<(object Key, object? Value)> Convert(object obj)
    {
        var fields = obj.GetType()
            .GetFields()
            .Select(f => (f.Name, f.GetValue(obj)));
        var properties = obj.GetType()
            .GetProperties()
            .Select(p => (p.Name, p.GetValue(obj)));

        return fields.Concat(properties)
            .Select(x => ((object)x.Name, x.Item2));
    }
}
