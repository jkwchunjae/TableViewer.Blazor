using System.Collections;
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
        var properties = obj.GetType().GetProperties()
            .Where(p => p.Name != "Parser")
            .Where(p => p.Name != "Descriptor")
            ;

        foreach (var property in properties.Where(p => p.PropertyType != typeof(Type)))
        {
            yield return (property.Name, property.GetValue(obj));
        }

        //var fields = obj.GetType().GetFields();
        //foreach (var field in fields.Where(f => f.IsPublic).Where(f => f.FieldType != typeof(Type)))
        //{
        //    yield return (field.Name, field.GetValue(obj));
        //}
    }
}
