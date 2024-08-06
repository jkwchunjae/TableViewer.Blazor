namespace TableViewerBlazor.Options;

public class TeObjectListAttribute : TeFieldAttribute
{
    public TeObjectListAttribute(string id)
        : base(id)
    {
    }
}

public interface ITeObjectListEditorOption : ITeFieldOption
{
    ITvAction AddItemAction { get; }
    ITvAction RemoveItemAction { get; }
}

public class TeObjectListEditorOption : ITeFieldOption
{
    public string? Id { get; set; }
    public ITvAction AddItemAction { get; set; } = CreateDefaultAddAction();
    public ITvAction RemoveItemAction { get; set; } = CreateDefaultRemoveAction();

    private static TvAction<object> CreateDefaultAddAction()
    {
        return new TvAction<object>
        {
            Action = _ => Task.CompletedTask,
            Label = "Add Item",
            Style = new TvButtonStyle
            {
                Size = Size.Medium,
                Dense = false,
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
            },
        };
    }
}

public class TeObjectListEditorOption<TValue> : ITeFieldOption, ITeGenericTypeOption
{
    public string? Id { get; set; }
    public ITvAction AddItemAction { get; set; } = CreateDefaultAddAction();
    public ITvAction RemoveItemAction { get; set; } = CreateDefaultRemoveAction();
    public string TypeName => typeof(TValue).Name;

    private static TvAction<object> CreateDefaultAddAction()
    {
        return new TvAction<object>
        {
            Action = _ => Task.CompletedTask,
            Label = "Add Item",
            Style = new TvButtonStyle
            {
                Size = Size.Medium,
                Dense = false,
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
            },
        };
    }
}
