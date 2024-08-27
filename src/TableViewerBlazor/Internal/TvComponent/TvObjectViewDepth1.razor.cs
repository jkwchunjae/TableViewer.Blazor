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

        Items = GetKeys(data)
            .Select(keyInfo => (keyInfo.Key, GetValue(data, keyInfo.MemberInfo), keyInfo.MemberInfo))
            .ToArray();
    }

    private IEnumerable<(string Key, MemberInfo MemberInfo)> GetKeys(object data)
    {
        if (data == null)
            yield break;

        var dataType = data.GetType();

        if (dataType.GetProperties().Length > 0)
        {
            var properties = dataType.GetProperties()
                .Where(p => p.CanRead)
                .Where(p => p.PropertyType != typeof(Type))
                .Where(p => p.GetCustomAttribute<TvIgnoreAttribute>() == null);
            foreach (var property in properties)
            {
                yield return (property.Name, property);
            }
        }

        if (dataType.GetFields().Length > 0)
        {
            var fields = dataType.GetFields()
                .Where(f => f.IsPublic)
                .Where(f => f.FieldType != typeof(Type))
                .Where(f => f.GetCustomAttribute<TvIgnoreAttribute>() == null);
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

}
