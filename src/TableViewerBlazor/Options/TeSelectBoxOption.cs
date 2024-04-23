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

public interface ITeSelectBoxOption : ITeFieldOption
{
    IEnumerable<ITeSelectItem> Items { get; }
}

public class TeSelectBoxOption<T> : TeFieldOption<T>, ITeSelectBoxOption
{
    public IEnumerable<ITeSelectItem> Items { get; set; } = new List<TeSelectItem<T>>();
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

