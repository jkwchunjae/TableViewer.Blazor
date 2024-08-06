namespace TableViewerBlazor.Options;

public class TeListEditorAttribute : TeFieldAttribute
{
    public TeListEditorAttribute(string id)
        : base(id)
    {
    }
}

public interface ITeListEditorOption : ITeConvertableFieldOption<object, IList>
{
    bool ShowNumber { get; }
    Func<object> CreateNew { get; }
    ITvAction AddItemAction { get; }
    ITvAction RemoveItemAction { get; }
}

public class TeListEditorOption<TValue, TListField, TListItem> : ITeListEditorOption, ITeGenericTypeOption
    where TListField : IList<TListItem>
{
    public string? Id { get; set; }
    public bool ShowNumber { get; set; } = false;
    public required Func<TListItem> CreateNew { get; init; }
    public ITvAction AddItemAction { get; set; } = CreateDefaultAddAction();
    public ITvAction RemoveItemAction { get; set; } = CreateDefaultRemoveAction();
    public required TeListEditorConverter<TValue, TListField, TListItem> Converter { get; set; }
    public string TypeName => typeof(TValue).Name;

    Func<object> ITeListEditorOption.CreateNew => () => CreateNew()!;
    ITeConverter ITeConvertable.Converter => Converter;
    ITeConverter<object, IList> ITeConvertableFieldOption<object, IList>.Converter => new TeConverter<object, IList>
    {
        ToField = userValue => userValue is TValue value ? Converter.ToField(value).Cast<object>().ToList() : default,
        //FromField = fieldValue => fieldValue is TListField value ? Converter.FromField(value) : default,
        FromField = fieldValue =>
        {
            if (fieldValue.Cast<TListItem>().ToList() is TListField value)
            {
                return Converter.FromField(value);
            }
            else
            {
                return default;
            }
        }
    };

    private static TvAction<object> CreateDefaultAddAction()
    {
        return new TvAction<object>
        {
            Action = _ => Task.CompletedTask,
            Label = "Add Item",
            Style = new TvButtonStyle
            {
                Size = Size.Medium,
                Dense = true,
                SuperDense = false,
            },
        };
    }

    private static TvAction<object> CreateDefaultRemoveAction()
    {
        return new TvAction<object>
        {
            Action = _ => Task.CompletedTask,
            Label = string.Empty,
            Style = new TvButtonStyle
            {
                StartIcon = Icons.Material.Outlined.Cancel,
                IconColor = Color.Error,
                Dense = false,
                SuperDense = false,
            }
        };
    }
}

public class TeListEditorOption<TListItem> : ITeListEditorOption, ITeGenericTypeOption
{
    public string? Id { get; set; }
    public bool ShowNumber { get; set; } = false;
    public required Func<TListItem> CreateNew { get; init; }
    public ITvAction AddItemAction { get; set; } = CreateDefaultAddAction();
    public ITvAction RemoveItemAction { get; set; } = CreateDefaultRemoveAction();
    public string TypeName => typeof(TListItem).Name;

    Func<object> ITeListEditorOption.CreateNew => () => CreateNew()!;
    ITeConverter ITeConvertable.Converter => new TeListEditorConverter<List<TListItem>>();
    ITeConverter<object, IList> ITeConvertableFieldOption<object, IList>.Converter => new TeConverter<object, IList>
    {
        ToField = userValue => userValue as IList,
        //FromField = fieldValue => fieldValue,
        FromField = fieldValue =>
        {
            if (fieldValue.Cast<TListItem>().ToList() is IList<TListItem> value)
            {
                return value;
            }
            else
            {
                return default;
            }
        }
    };

    public static TeListEditorOption<TListItem> Create(TListItem defaultValue)
    {
        return new TeListEditorOption<TListItem>()
        {
            CreateNew = () => defaultValue,
        };
    }

    private static TvAction<object> CreateDefaultAddAction()
    {
        return new TvAction<object>
        {
            Action = _ => Task.CompletedTask,
            Label = "Add Item",
            Style = new TvButtonStyle
            {
                Size = Size.Medium,
                Dense = true,
                SuperDense = false,
            },
        };
    }

    private static TvAction<object> CreateDefaultRemoveAction()
    {
        return new TvAction<object>
        {
            Action = _ => Task.CompletedTask,
            Label = string.Empty,
            Style = new TvButtonStyle
            {
                StartIcon = Icons.Material.Outlined.Cancel,
                IconColor = Color.Error,
                Dense = false,
                SuperDense = false,
            }
        };
    }
}

public class TeListEditorConverter<TValue, TListField> : ITeConverter<TValue, TListField>
    where TListField : IList
{
    public required Func<TValue, TListField> ToList { get; set; }
    public required Func<TListField, TValue?> FromList { get; set; }

    public Func<TValue, TListField> ToField => userValue => ToList(userValue);
    public Func<TListField, TValue?> FromField => fieldValue => FromList(fieldValue);
    Func<object, object?> ITeConverter.ToField => userValue => userValue is TValue value ? ToList(value) : default;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue is TListField value ? FromList(value) : default;
}

public class TeListEditorConverter<TValue, TListField, TListItem> : ITeConverter<TValue, TListField>
    where TListField : IList<TListItem>
{
    public required Func<TValue, TListField> ToList { get; set; }
    public required Func<TListField, TValue?> FromList { get; set; }

    public Func<TValue, TListField> ToField => userValue => ToList(userValue);
    public Func<TListField, TValue?> FromField => fieldValue => FromList(fieldValue);
    Func<object, object?> ITeConverter.ToField => userValue => userValue is TValue value ? ToList(value) : default;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue is TListField value ? FromList(value) : default;
}

public class TeListEditorConverter<TList> : ITeConverter<TList, TList>
    where TList : IList
{
    public Func<TList, TList> ToField => userValue => userValue;
    public Func<TList, TList> FromField => fieldValue => fieldValue;
    Func<object, object?> ITeConverter.ToField => userValue => userValue;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue;
}

