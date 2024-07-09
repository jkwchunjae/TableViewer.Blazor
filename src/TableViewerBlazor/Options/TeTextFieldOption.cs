using TableViewerBlazor.Internal.TeComponent;
using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public static class TeTextFieldOptionExtensions
{
    public static bool TryGetTextFieldOption(this TeOptions options,
        MemberInfo? memberInfo, TeEditorBase teBase,
        out ITeTextFieldOption? textFieldOption)
    {
        var textFieldAttribute = memberInfo?.GetCustomAttribute<TeTextFieldAttribute>();
        if (textFieldAttribute != null)
        {
            textFieldOption = options.TextFieldOptions?
                .FirstOrDefault(o => o.Id == textFieldAttribute.Id) ?? default;
            if (textFieldOption != null)
            {
                return true;
            }
        }

        textFieldOption = options.TextFieldOptions?
            .Where(option => string.IsNullOrEmpty(option.Id))
            .FirstOrDefault(o => o.Condition?.Invoke(teBase.Data, teBase.Depth, teBase.Path) ?? true) ?? default;
        if (textFieldOption != null)
        {
            return true;
        }

        textFieldOption = teBase.Data switch
        {
            string => new TeTextFieldOption<string>(),
            _ => null,
        };
        if (teBase.Data == null && memberInfo?.MemberType() == typeof(string))
        {
            textFieldOption = new TeTextFieldOption<string>();
        }
        return textFieldOption != null;
    }

    public static bool TryGetTextConvertableFieldOption(this TeOptions options,
        MemberInfo? memberInfo, TeEditorBase teBase,
        out ITeTextFieldOption? textFieldOption)
    {
        if (options.TryGetTextFieldOption(memberInfo, teBase, out textFieldOption))
        {
            if (textFieldOption?.Converter != null)
            {
                return true;
            }
        }
        return false;
    }
}

public class TeTextFieldAttribute : Attribute
{
    public string Id { get; init; }
    public TeTextFieldAttribute(string id)
    {
        Id = id;
    }
}

public interface ITeTextFieldOption : ITeFieldOption
{
    IEnumerable<ITeValidation> Validations { get; }
    ITeTextFieldProperty? Property { get; }
    ITeTextFieldEvent? Event { get; }
    ITeTextFieldConverter? Converter { get; }
}

public class TeTextFieldOption<T> : ITeFieldOption<T>, ITeTextFieldOption
{
    public string? Id { get; set; }
    public Func<T?, int, string, bool>? Condition { get; set; }
    public List<ITeValidation> Validations { get; set; } = [];
    public TeTextFieldProperty? Property { get; set; }
    public TeTextFieldEvent<T>? Event { get; set; }
    public TeTextFieldConverter<T>? Converter { get; set; }

    IEnumerable<ITeValidation> ITeTextFieldOption.Validations => Validations;
    ITeTextFieldProperty? ITeTextFieldOption.Property => Property;
    ITeTextFieldEvent? ITeTextFieldOption.Event => Event;
    ITeTextFieldConverter? ITeTextFieldOption.Converter => Converter;
}

public interface ITeTextFieldConverter
{
    Func<object, string> StringValue { get; }
    Func<string, object> FromString { get; }
}

public class TeTextFieldConverter<T> : ITeTextFieldConverter
{
    public required Func<T, string> StringValue { get; init; }
    public required Func<string, T> FromString { get; init; }

    Func<object, string> ITeTextFieldConverter.StringValue => o => o is T t ? StringValue(t) : throw new Exception();
    Func<string, object> ITeTextFieldConverter.FromString => value => FromString(value)!;
}