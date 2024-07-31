namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeObjectArrayEditor : TeEditorBase
{
    [Parameter] public IList Items { get; set; } = default!;
    private IEnumerable<object> ItemsEnumerable => Items.Cast<object>();

    private MemberInfo[] MemberInfos = Array.Empty<MemberInfo>();

    private List<ICustomEditorArgument> CustomEditorArguments = new List<ICustomEditorArgument>();

    protected override void OnInitialized()
    {
        MemberInfos = GetMembers(Items.GetType().GenericTypeArguments[0]);
    }

    private MemberInfo[] GetMembers(Type itemType)
    {
        var members = new List<MemberInfo>();
        members.AddRange(itemType.GetProperties());
        members.AddRange(itemType.GetFields());
        return members
            .Where(member => member.GetCustomAttribute<TeIgnoreAttribute>() == null)
            .ToArray();
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

        foreach (var argument in CustomEditorArguments.Where(x => x.Parent == item))
        {
            argument.Update(item);
        }
    }

    private async Task AddItem()
    {
        var itemType = Items.GetType().GenericTypeArguments[0];
        var item = Activator.CreateInstance(itemType);
        Items.Add(item);
        await DataChanged.InvokeAsync(Items);
    }

    private async Task RemoveItem(object item)
    {
        Items.Remove(item);
        await DataChanged.InvokeAsync(Items);
    }

    private ICustomEditorArgument GetCustomEditorArgument(object? value, object parent, MemberInfo memberInfo)
    {
        var argument = CustomEditorArguments.FirstOrDefault(x => x.Parent == parent);

        if (argument != default)
        {
            return argument;
        }
        else
        {
            argument = new CustomEditorArguemnt
            {
                Value = value,
                Parent = parent,
                DataChanged = value => OnDataChanged(parent, memberInfo, value),
            };
            CustomEditorArguments.Add(argument);

            return argument;
        }
    }
}
