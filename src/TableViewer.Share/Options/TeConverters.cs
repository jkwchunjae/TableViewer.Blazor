using System.Collections;
using System.Numerics;

namespace TableViewer.Share.Options;

public class TeBoolConverter<TValue> : ITeConverter<TValue, bool>
{
    public required Func<TValue, bool> ToBoolean { get; set; }
    public required Func<bool, TValue?> FromBoolean { get; set; }

    public Func<TValue, bool> ToField => userValue => ToBoolean(userValue);
    public Func<bool, TValue?> FromField => fieldValue => FromBoolean(fieldValue);
    Func<object, object?> ITeConverter.ToField => userValue => userValue is TValue value ? ToBoolean(value) : false;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue is bool value ? FromBoolean(value) : default;
}

public class TeBoolConverter : ITeConverter<bool, bool>
{
    public Func<bool, bool> ToField => userValue => userValue;
    public Func<bool, bool> FromField => fieldValue => fieldValue;
    Func<object, object?> ITeConverter.ToField => userValue => userValue;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue;
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

public class TeNumericFieldConverter<TValue, TNumber> : ITeConverter<TValue, TNumber>
    where TNumber: INumber<TNumber>, IMinMaxValue<TNumber>
{
    public required Func<TValue, TNumber?> ToNumber { get; set; }
    public required Func<TNumber, TValue?> FromNumber { get; set; }

    public Func<TValue, TNumber?> ToField => userValue => ToNumber(userValue);
    public Func<TNumber, TValue?> FromField => fieldValue => FromNumber(fieldValue);
    Func<object, object?> ITeConverter.ToField => userValue => userValue is TValue value ? ToNumber(value) : TNumber.MinValue;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue is TNumber value ? FromNumber(value) : default(TValue);
}

public class TeNumericFieldConverter<TNumber> : ITeConverter<TNumber, TNumber>
    where TNumber: INumber<TNumber>, IMinMaxValue<TNumber>
{
    public Func<TNumber, TNumber?> ToField => userValue => userValue;
    public Func<TNumber, TNumber?> FromField => fieldValue => fieldValue;
    Func<object, object?> ITeConverter.ToField => userValue => userValue is TNumber TNumberValue ? ToField(TNumberValue) : TNumber.MinValue;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue is TNumber TNumberValue ? FromField(TNumberValue) : TNumber.MinValue;
}

public class TeTextFieldConverter<TValue> : ITeConverter<TValue, string>
{
    public required Func<TValue, string?> StringValue { get; set; }
    public required Func<string, TValue?> FromString { get; set; }

    public Func<TValue, string?> ToField => userValue => StringValue(userValue);
    public Func<string, TValue?> FromField => fieldValue => FromString(fieldValue);
    Func<object, object?> ITeConverter.ToField => userValue => userValue is TValue value ? StringValue(value) : string.Empty;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue is string value ? FromString(value) : default;
}

public class TeTextFieldConverter : ITeConverter<string, string>
{
    public Func<string, string?> ToField => userValue => userValue;
    public Func<string, string?> FromField => fieldValue => fieldValue;
    Func<object, object?> ITeConverter.ToField => userValue => userValue is string stringValue ? ToField(stringValue) : string.Empty;
    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue is string stringValue ? FromField(stringValue) : string.Empty;
}
