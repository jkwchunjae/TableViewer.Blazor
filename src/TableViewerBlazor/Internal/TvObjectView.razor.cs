using System.Collections;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace TableViewerBlazor.Internal;

public partial class TvObjectView : TvViewBase
{
    [Parameter] public object? Data { get; set; }

    bool Open = false;

    private string[] Keys = Array.Empty<string>();
    private IEnumerable<(string Key, object? Value)> Items = Enumerable.Empty<(string, object?)>();


    protected override void OnInitialized()
    {
        if (Options != null)
        {
            Open = Depth <= Options.OpenDepth;
        }
        if (Data != null)
        {
            var keys = GetKeys(Data);

            var columnOption = Options?.ColumnVisible?.FirstOrDefault(x => x.Matched(keys, keyInfo => keyInfo.Key));
            if (columnOption != null)
            {
                keys = columnOption.NewKeys(keys, keyInfo => keyInfo.Key);
            }

            if (Options?.DisableKeys?.Any() ?? false)
            {
                keys = keys
                    .Where(key => Options!.DisableKeys!.All(disable => disable != key.Key));
            }

            Items = keys
                .Select(keyInfo => (keyInfo.Key, GetValue(Data, keyInfo.MemberInfo)))
                .ToArray();
        }
    }

    private IEnumerable<(string Key, MemberInfo MemberInfo)> GetKeys(object data)
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
                yield return (property.Name, property);
            }
        }

        if (Options?.ReadField ?? false)
        {
            var fields = dataType.GetFields()
                .Where(f => f.IsPublic)
                .Where(f => f.FieldType != typeof(Type));
            foreach (var field in fields)
            {
                yield return (field.Name, field);
            }
        }
    }

    private object? GetValue(object obj, MemberInfo memberInfo)
    {
        return memberInfo switch
        {
            PropertyInfo property => property.GetValue(obj, null),
            FieldInfo field => field.GetValue(obj),
            _ => null,
        };
    }

    private void ToggleOpen()
    {
        Open = !Open;
    }
}
