using Microsoft.JSInterop;

namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeObjectArrayEditor : TeEditorBase
{
    [Parameter] public IEnumerable<object> Items { get; set; } = default!;
    private bool Addable { get; } = true;

    private MemberInfo[] MemberInfos = Array.Empty<MemberInfo>();

    protected override void OnInitialized()
    {
        MemberInfos = GetMembers(Items.GetType().GenericTypeArguments[0]);
    }

    private MemberInfo[] GetMembers(Type itemType)
    {
        var members = new List<MemberInfo>();
        members.AddRange(itemType.GetProperties());
        members.AddRange(itemType.GetFields());
        return members.ToArray();
    }

    private object? GetValue(object? item, MemberInfo memberInfo)
    {
        if (memberInfo is PropertyInfo property)
        {
            return property.GetValue(item);
        }
        if (memberInfo is FieldInfo field)
        {
            return field.GetValue(item);
        }
        return null;
    }

    private async Task OnDataChanged(object? item, MemberInfo memberInfo, object? value)
    {
        if (item == null)
        {
            return;
        }
        if (memberInfo is PropertyInfo property)
        {
            property.SetValue(item, value);
        }
        if (memberInfo is FieldInfo field)
        {
            field.SetValue(item, value);
        }
        await DataChanged.InvokeAsync(Items);
    }
}
