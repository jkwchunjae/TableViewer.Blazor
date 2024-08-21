using System.ComponentModel;
using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public class TeAutocompleteAttribute : TeFieldAttribute
{
    public TeAutocompleteAttribute(string id) : base(id) { }
}

//public interface ITeAutocompleteOption : ITeConvertableFieldOption<object, string>
//{
//    public TeAutocompleteProperty Property { get; }
//}

//public class TeAutocompleteOption<TValue> : ITeAutocompleteOption
//{
//    public string? Id { get; set; }

//    public TeAutocompleteProperty Property { get; set; } = new();
//    public required TeStringConverter<TValue> Converter { get; set; }
//    ITeConverter ITeConvertable.Converter => Converter;

//    ITeConverter<object, string> ITeConvertableFieldOption<object, string>.Converter => new TeConverter<object, string>
//    {
//        ToField = userValue => userValue is TValue value ? Converter.ToStringValue(value) : string.Empty,
//        FromField = fieldValue => fieldValue is string value ? Converter.FromStringValue(value) : default,
//    };

//}

//public class TeStringConverter<TValue> : ITeConverter<TValue, string>
//{
//    public required Func<TValue, string> ToStringValue { get; set; }
//    public required Func<string, TValue> FromStringValue { get; set; }

//    public Func<TValue, string> ToField => userValue => ToStringValue(userValue);
//    public Func<string, TValue> FromField => fieldValue => FromStringValue(fieldValue);
//    Func<object, object?> ITeConverter.ToField => userValue => userValue is TValue value ? ToStringValue(value) : string.Empty;
//    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue is string value ? FromStringValue(value) : default;
//}

public interface ITeAutocompleteOption : ITeFieldOption
{
    public TeAutocompleteProperty Property { get; }
    public Func<object, string> StringConverter { get; }
}

public class TeAutocompleteOption<TValue> : ITeAutocompleteOption
{
    public string? Id { get; set; }
    public TeAutocompleteProperty Property { get; set; } = new();
    public Func<TValue, string>? StringConverter { get; set; }
    Func<object, string> ITeAutocompleteOption.StringConverter => obj => obj is TValue value && StringConverter != null ? StringConverter(value) : obj.ToString() ?? string.Empty;
}