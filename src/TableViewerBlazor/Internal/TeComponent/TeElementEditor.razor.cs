namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeElementEditor : TeEditorBase
{
    /// <summary>
    /// FieldInfo, PropertyInfo
    /// </summary>
    [Parameter] public MemberInfo? MemberInfo { get; set; }
    [Parameter] public object? Parent { get; set; }

    private static bool HasOnlyPrimitiveElements(IList items)
    {
        return items.Cast<object>().All(item =>
        {
            return item is string
                || item is char
                || IsNumber(item)
                || item is bool;
        });
    }

    private static bool IsNumber(object element)
    { 
        return element switch
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
    }
}
