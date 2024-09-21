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
    string Title { get; }
    bool EnableAddItem => AddItemAction != null;
    bool EnableRemoveItem => RemoveItemAction != null;
    ITvAction? AddItemAction { get; }
    ITvAction? RemoveItemAction { get; }
}

public class TeObjectListEditorOption<TListItem> : ITeObjectListEditorOption, ITeGenericTypeOption
{
    public string? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public TvAction<List<TListItem>>? AddItemAction { get; set; }
    public ITvAction? RemoveItemAction { get; set; }
    public string TypeName => typeof(List<TListItem>).FullName!;

    ITvAction? ITeObjectListEditorOption.AddItemAction => AddItemAction;

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
