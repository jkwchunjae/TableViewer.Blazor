namespace TableViewerBlazor.Options;

public interface ITeFieldOption
{
    string? Id { get; }
}

public interface ITeFieldOption<TValue> : ITeFieldOption
{
}

public abstract class TeFieldAttribute : Attribute
{
    public string Id { get; init; }
    public TeFieldAttribute(string id)
    {
        Id = id;
    }
}

public interface ITeGenericTypeOption : ITeFieldOption
{
    string TypeName { get; }
}

public interface ITeConvertable : ITeFieldOption
{
    ITeConverter Converter { get; }
}

public interface ITeConvertableFieldOption<TValue, TField> : ITeConvertable
{
    new ITeConverter<TValue, TField> Converter { get; }
}

public interface ITeConverter
{
    Func<object, object?> ToField { get; }
    Func<object, object?> FromField { get; }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="TValue">유저가 가지고 있는 데이터 타입</typeparam>
/// <typeparam name="TField">TableEditor쪽 데이터 타입</typeparam>
public interface ITeConverter<TValue, TField> : ITeConverter
{
    /// <summary> 유저가 가지고 있는 데이터 타입을 TableEditor쪽 데이터 타입으로 변환 </summary>
    new Func<TValue, TField?> ToField { get; }
    /// <summary> TableEditor쪽 데이터를 유저가 가지고 있는 데이터 타입으로 변환 </summary>
    new Func<TField, TValue?> FromField { get; }
}

public class TeConverter<TValue, TField> : ITeConverter<TValue, TField>
{
    public required Func<TValue, TField?> ToField { get; set; }

    public required Func<TField, TValue?> FromField { get; set; }

    Func<object, object?> ITeConverter.ToField => userValue => userValue is TValue value ? ToField(value) : default;

    Func<object, object?> ITeConverter.FromField => fieldValue => fieldValue is TField value ? FromField(value) : default;
}