namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeObjectListEditor : TeEditorBase
{
    [Parameter] public ITeObjectListEditorOption ObjectListOption { get; set; } = default!;
    private IList Items { get; set; } = default!;
    private IEnumerable<(int index, object obj)> ItemsEnumerable => Items
        .Cast<object>()
        .Select((item, index) => (index, item));
    private ITvAction AddItemAction => new TvAction<object>
    {
        Action = _ => AddItem(),
        Label = ObjectListOption.AddItemAction?.Label ?? string.Empty,
        Style = ObjectListOption.AddItemAction?.Style ?? new(),
        LabelAfterClick = ObjectListOption.AddItemAction?.LabelAfterClick ?? string.Empty,
    };
    private ITvAction RemoveItemAction => new TvAction<object>
    {
        Action = item => RemoveItem(item),
        Label = ObjectListOption.RemoveItemAction?.Label ?? string.Empty,
        Style = ObjectListOption.RemoveItemAction?.Style ?? new(),
        LabelAfterClick = ObjectListOption.RemoveItemAction?.LabelAfterClick ?? string.Empty,
    };

    private MemberInfo[] MemberInfos = Array.Empty<MemberInfo>();
    private List<ICustomEditorArgument> CustomEditorArguments = new List<ICustomEditorArgument>();

    protected override void OnInitialized()
    {
        if (Data is IList listData)
        {
            Items = listData;
        }
        Type objectType = GetInnerObjectType(Items);
        MemberInfos = GetMembers(objectType);

        Type GetInnerObjectType(IList items)
        {
            if (items.GetType().GenericTypeArguments.Length > 0)
            {
                return items.GetType().GenericTypeArguments[0];
            }
            else
            {
                return items.GetType().BaseType!.GenericTypeArguments[0];
            }
        }
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

    private async Task OnDataChanged((int index, object item) itemWithIndex, MemberInfo memberInfo, object? value)
    {
        var (index, item) = itemWithIndex;
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
        Items[index] = item;
        await DataChanged.InvokeAsync(Items);

        foreach (var argument in CustomEditorArguments.Where(x => x.Parent == item))
        {
            argument.OnParentChanged(item);
        }
    }

    private async Task AddItem()
    {
        await ObjectListOption.AddItemAction!.Action.Invoke(Items);
        await DataChanged.InvokeAsync(Items);
    }

    private async Task RemoveItem(object item)
    {
        Items.Remove(item);
        await DataChanged.InvokeAsync(Items);
    }

    private ICustomEditorArgument GetCustomEditorArgument(object? value, (int index, object parent) parentWithIndex, MemberInfo memberInfo)
    {
        var argument = CustomEditorArguments.FirstOrDefault(x => x.Value == value && x.Parent == parentWithIndex.parent);

        if (argument != default)
        {
            return argument;
        }
        else
        {
            argument = new CustomEditorArgument<object, object>
            {
                Value = value,
                Parent = parentWithIndex.parent,
                DataChanged = value => OnDataChanged(parentWithIndex, memberInfo, value),
            };
            CustomEditorArguments.Add(argument);
            return argument;
        }
    }
}