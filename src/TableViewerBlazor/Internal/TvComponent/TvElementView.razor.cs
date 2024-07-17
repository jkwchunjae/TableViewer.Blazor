using TableViewerBlazor.Public;

namespace TableViewerBlazor.Internal.TvComponent;

public partial class TvElementView : TvViewBase
{
    [Parameter] public object? Parent { get; set; }
    [Parameter] public object? Data { get; set; }
    [Parameter] public MemberInfo? MemberInfo { get; set; }

    private bool IsNumber => Data switch
    {
        // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types
        sbyte => true,
        byte => true,
        short => true,
        ushort => true,
        int => true,
        uint => true,
        long => true,
        ulong => true,
        nint => true,
        nuint => true,
        // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types
        float => true,
        double => true,
        decimal => true,
        _ => false,
    };

    private bool IsObjectArray => Data.IsObjectArray();
    private bool UseObjectArray
    {
        get
        {
            if (UseEditorInElement(Data!))
            {
                return false;
            }
            return true;
        }
    }

    private bool UseEditorInElement(object data)
    {
        var dataType = data.GetType();
        var elementType = GetElementType(dataType);

        if (elementType == null)
            return false;

        var option = Options.Editor?.FirstOrDefault(option => option.ConditionByType(elementType, Depth + 1, "path"));

        return option != null;

        Type? GetElementType(Type type)
        {
            if (type.IsArray)
            {
                var elementType = type.GetElementType()!;
                return elementType;
            }
            else // if (arr is IEnumerable)
            {
                if (type.GenericTypeArguments.Length == 1)
                {
                    return type.GenericTypeArguments[0];
                }
            }
            return null;
        }
    }

    private bool DataHasStringViewerAttribute()
    {
        if (MemberInfo == null)
            return false;

        var memberAttr = MemberInfo.GetCustomAttribute<TvStringAttribute>();

        if (memberAttr != null)
        {
            return true;
        }

        return MemberInfo switch
        {
            PropertyInfo propertyInfo => propertyInfo.PropertyType.GetCustomAttribute<TvStringAttribute>() != null,
            FieldInfo fieldInfo => fieldInfo.FieldType.GetCustomAttribute<TvStringAttribute>() != null,
            _ => false,
        };
    }
}
