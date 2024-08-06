using TableViewerBlazor.Internal.TeComponent;
using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public static class TeSelectBoxOptionExtensions
{
    public static bool TryGetSelectBoxOption(this TeOptions options,
        MemberInfo? memberInfo, TeEditorBase teBase,
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
        selectBoxOption = default;
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
    ITeSelectBoxProperty? Property { get; }
}

public class TeSelectBoxOption<TValue> : ITeSelectBoxOption
{
    public string? Id { get; set; }
    public List<TeSelectItem<TValue>> Items { get; set; } = [];
    public TeSelectBoxProperty<TValue>? Property { get; set; }

    IEnumerable<ITeSelectItem> ITeSelectBoxOption.Items => Items;
    ITeSelectBoxProperty? ITeSelectBoxOption.Property => Property;
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
