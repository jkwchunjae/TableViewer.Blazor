namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeObjectEditor : TeEditorBase
{
    private IEnumerable<(string Key, MemberInfo MemberInfo, object? Value)> GetKeys(object data)
    {
        if (data == null)
            yield break;

        var dataType = data.GetType();

        if (Options?.ReadProperty ?? false)
        {
            var properties = dataType.GetProperties()
                .Where(p => p.CanRead)
                .Where(p => p.CanWrite)
                .Where(p => p.PropertyType != typeof(Type));
            foreach (var property in properties)
            {
                yield return (property.Name, property, property.GetValue(data));
            }
        }

        if (Options?.ReadField ?? false)
        {
            var fields = dataType.GetFields()
                .Where(f => f.IsPublic)
                .Where(f => f.FieldType != typeof(Type));
            foreach (var field in fields)
            {
                yield return (field.Name, field, field.GetValue(data));
            }
        }
    }

    private Task OnDataChanged(MemberInfo memberInfo, object? value)
    {
        return memberInfo switch
        {
            PropertyInfo propertyInfo => OnDataChanged_Property(propertyInfo, value),
            FieldInfo fieldInfo => OnDataChanged_Field(fieldInfo, value),
            _ => Task.CompletedTask,
        };
    }

    private async Task OnDataChanged_Property(PropertyInfo property, object? value)
    {
        property.SetValue(Data, value);
        await DataChanged.InvokeAsync(Data);
    }

    private async Task OnDataChanged_Field(FieldInfo field, object? value)
    {
        field.SetValue(Data, value);
        await DataChanged.InvokeAsync(Data);
    }
}
