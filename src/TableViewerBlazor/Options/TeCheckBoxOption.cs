using TableViewerBlazor.Internal.TeComponent;
using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public static class TeCheckBoxOptionExtensions
{
    public static bool TryGetCheckBoxOption(
        this TeOptions options,
        MemberInfo? memberInfo,
        TeEditorBase teBase,
        out ITeCheckBoxOption? checkBoxOption
        )
    {
        var selectedAttribute = memberInfo?.GetCustomAttribute<TeCheckBoxAttribute>();
        if (selectedAttribute != null)
        {
            checkBoxOption = options.CheckBoxOptions?
                .FirstOrDefault(o => o.Id == selectedAttribute.Id) ?? default;
            if (checkBoxOption != null)
            {
                return true;
            }
        }

        checkBoxOption = teBase.Data switch
        {
            bool _ => new TeCheckBoxOption(),
            _ => null
        };
        return checkBoxOption != null;
    }
}

public class TeCheckBoxAttribute : TeFieldAttribute
{
    public TeCheckBoxAttribute(string id)
        : base(id)
    {
    }
}

public interface ITeCheckBoxOption : ITeConvertableFieldOption<object, bool>
{
    public ITeCheckBoxProperty Property { get; } 
}

public class TeCheckBoxOption<TValue> : ITeCheckBoxOption
{
    public string? Id { get; set; }
    public ITeCheckBoxProperty Property { get; set; } = new TeCheckBoxProperty();
    public required TeCheckBoxConverter<TValue> Converter { get; set; }

    ITeConverter ITeConvertable.Converter => Converter;
    ITeConverter<object, bool> ITeConvertableFieldOption<object, bool>.Converter => new TeConverter<object, bool>
    {
        ToField = userValue => userValue is TValue value ? Converter.ToBoolean(value) : false,
        FromField = fieldValue => fieldValue is bool value ? Converter.FromBoolean(value) : default,
    };
}

public class TeCheckBoxOption : ITeCheckBoxOption
{
    public string? Id { get; set; }
    public ITeCheckBoxProperty Property { get; set; } = new TeCheckBoxProperty();

    ITeConverter ITeConvertable.Converter => new TeCheckBoxConverter();
    ITeConverter<object, bool> ITeConvertableFieldOption<object, bool>.Converter => new TeConverter<object, bool>
    {
        ToField = userValue => userValue is bool value ? value : false,
        FromField = fieldValue => fieldValue,
    };
}

public class TeCheckBoxConverter<TValue> : ITeConverter<TValue, bool>
{
    public required Func<TValue, bool> ToBoolean { get; set; }
    public required Func<bool, TValue?> FromBoolean { get; set; }

    public Func<TValue, bool> ToField => userValue => ToBoolean(userValue);
    public Func<bool, TValue?> FromField => fieldValue => FromBoolean(fieldValue);
    Func<object, object?> ITeConverter.ToField => userValue => userValue is TValue value ? ToBoolean(value) : false;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue is bool value ? FromBoolean(value) : default;
}

public class TeCheckBoxConverter : ITeConverter<bool, bool>
{
    public Func<bool, bool> ToField => userValue => userValue;
    public Func<bool, bool> FromField => fieldValue => fieldValue;
    Func<object, object?> ITeConverter.ToField => userValue => userValue;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue;
}