using TableViewerBlazor.Internal.TeComponent;
using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public static class TeRadioOptionExtensions
{
    public static bool TryGetRadioOption(this TeOptions options,
        MemberInfo? memberInfo, TeEditorBase teBase,
        out ITeRadioOption? radioOption)
    {
        var selectAttribute = memberInfo?.GetCustomAttribute<TeRadioAttribute>();
        if (selectAttribute != null)
        {
            radioOption = options.RadioOptions?
                .FirstOrDefault(o => o.Id == selectAttribute.Id) ?? default;
            if (radioOption != null)
            {
                return true;
            }
        }
        radioOption = options.RadioOptions?
            .Where(option => string.IsNullOrEmpty(option.Id))
            .Where(option => option.Condition?.Invoke(teBase.Data, teBase.Depth, teBase.Path) ?? true)
            .FirstOrDefault() ?? default;
        return radioOption != null;
    }
}

public class TeRadioAttribute : Attribute
{
    public string Id { get; init; }
    public TeRadioAttribute(string id)
    {
        Id = id;
    }
}

public interface ITeRadioOption : ITeFieldOption<object, object>
{
    IEnumerable<ITeValidation> Validations { get; }
    IEnumerable<ITeRadioItem> Items { get; }
    ITeRadioGroupProperty? Property { get; }
}

public class TeRadioOption<TValue> : ITeRadioOption
{
    public string? Id { get; set; }
    public Func<TValue?, int, string, bool>? Condition { get; set; }
    public List<ITeValidation> Validations { get; set; } = [];
    public required List<TeRadioItem<TValue>> Items { get; set; }
    public TeRadioGroupProperty? Property { get; set; }
    public ITeConverter<object, object> Converter => new TeConverter<object, object>
    {
        ToField = value => value,
        FromField = value => value,
    };
    IEnumerable<ITeValidation> ITeRadioOption.Validations => Validations;
    IEnumerable<ITeRadioItem> ITeRadioOption.Items => Items;
    ITeRadioGroupProperty? ITeRadioOption.Property => Property;
    ITeConverter ITeFieldOption.Converter => Converter;
    Func<object?, int, string, bool>? ITeFieldOption.Condition =>
        (obj, depth, path) =>
        {
            if (obj is TValue value)
            {
                return Condition?.Invoke(value, depth, path) ?? true;
            }
            else
            {
                return false;
            }
        };
}

public interface ITeRadioItem
{
    object? Value { get; }
    string Text { get; }
    bool Default { get; }
    ITeRadioItemProperty? Property { get; }
}

public record TeRadioItem<TValue> : ITeRadioItem
{
    public TValue? Value { get; init; } = default!;
    public string Text { get; init; } = string.Empty;
    public bool Default { get; init; } = false;
    public TeRadioItemProperty? Property { get; set; }

    object? ITeRadioItem.Value => Value;
    ITeRadioItemProperty? ITeRadioItem.Property => Property;

    public TeRadioItem()
    {
    }
    public TeRadioItem(TValue value, string text, bool @default = false)
    {
        Value = value;
        Text = text;
        Default = @default;
    }
}
