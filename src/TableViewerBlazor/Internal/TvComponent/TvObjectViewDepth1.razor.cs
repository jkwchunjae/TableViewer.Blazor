namespace TableViewerBlazor.Internal.TvComponent;

public partial class TvObjectViewDepth1 : TvViewBase
{
    [Parameter] public object? Data { get; set; }

    private string[] Keys = Array.Empty<string>();
    private IEnumerable<(string Key, object? Value, MemberInfo MemberInfo)> Items = Enumerable.Empty<(string, object?, MemberInfo)>();

    protected override void OnParametersSet()
    {
        if (Data != null)
        {
            UpdateData(Data);
        }
        StateHasChanged();
    }

    private void UpdateData(object data)
    {
        var keys = GetKeys(data);

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
            .Select(keyInfo => (keyInfo.Key, GetValue(data, keyInfo.MemberInfo), keyInfo.MemberInfo))
            .ToArray();
    }

    private IEnumerable<(string Key, MemberInfo MemberInfo)> GetKeys(object data)
    {
        if (data == null)
            yield break;

        var dataType = data.GetType();

        var properties = dataType.GetProperties()
            .Where(p => p.CanRead)
            .Where(p => p.PropertyType != typeof(Type));
        foreach (var property in properties)
        {
            yield return (property.Name, property);
        }

        var fields = dataType.GetFields()
            .Where(f => f.IsPublic)
            .Where(f => f.FieldType != typeof(Type));
        foreach (var field in fields)
        {
            yield return (field.Name, field);
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

}
