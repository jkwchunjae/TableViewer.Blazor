namespace TableViewerBlazor.Options;

public static class TeSelectBoxOptionExtensions
{
    public static bool TryGetSelectBoxOption(this TeOptions options,
        MemberInfo? memberInfo, object? data,
        out ITeSelectBoxOption? selectBoxOption)
    {
        var selectAttribute = memberInfo?.GetCustomAttribute<TeSelectBoxAttribute>();
        if (selectAttribute != null)
        {
            selectBoxOption = options.SelectBoxOptions?
                .FirstOrDefault(o => o.Id == selectAttribute.Id) ?? default;
            if (selectBoxOption != null)
            {
                return true;
            }
        }
        selectBoxOption = options.SelectBoxOptions?
            .Where(option => string.IsNullOrEmpty(option.Id))
            .FirstOrDefault(o => o.Condition(data, 0, "path")) ?? default;
        return selectBoxOption != null;
    }
}

public class TeSelectBoxAttribute : Attribute
{
    public string Id { get; init; }
    public TeSelectBoxAttribute(string id)
    {
        Id = id;
    }
}

public interface ITeSelectBoxOption
{
    public string Id { get; init; }
    public IEnumerable<ITeSelectItem> Items { get; }
    public Func<object?, int, string, bool> Condition { get; }
}

public class TeSelectBoxOption<T> : ITeSelectBoxOption
{
    public required string Id { get; init; }
    public IEnumerable<TeSelectItem<T>> Items { get; set; } = new List<TeSelectItem<T>>();
    public Func<T?, int, string, bool> Condition { get; set; } = (value, index, path) => true;

    IEnumerable<ITeSelectItem> ITeSelectBoxOption.Items => Items;
    Func<object?, int, string, bool> ITeSelectBoxOption.Condition =>
        (value, index, path) =>
        {
            if (value is T tValue)
            {
                return Condition?.Invoke(tValue, index, path) ?? true;
            }
            else
            {
                return false;
            }
        };
}

public interface ITeSelectItem
{
    object? Value { get; }
    string Text { get; }
    bool Default { get; }
}

public record TeSelectItem<T> : ITeSelectItem
{
    public T? Value { get; init; } = default!;
    public string Text { get; init; } = string.Empty;
    public bool Default { get; init; } = false;

    object? ITeSelectItem.Value => Value;

    public TeSelectItem()
    {
    }
    public TeSelectItem(T value, string text, bool @default = false)
    {
        Value = value;
        Text = text;
        Default = @default;
    }
}

