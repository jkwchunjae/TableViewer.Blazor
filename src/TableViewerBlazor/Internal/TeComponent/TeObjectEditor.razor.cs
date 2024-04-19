namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeObjectEditor : ComponentBase
{
    /// <summary>
    /// FieldInfo, PropertyInfo
    /// </summary>
    [Parameter] public Type Type { get; set; } = default!;
    [Parameter] public object? Data { get; set; }
    [Parameter] public string? Name { get; set; }
    [Parameter] public TeOptions Options { get; set; } = new TeOptions();

    protected override void OnInitialized()
    {
        var keys = GetKeys(Data!);


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
}
