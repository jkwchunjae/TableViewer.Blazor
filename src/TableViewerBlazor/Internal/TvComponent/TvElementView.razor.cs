﻿using TableViewerBlazor.Public;

namespace TableViewerBlazor.Internal.TvComponent;

public partial class TvElementView : TvViewBase
{
    [Parameter] public object? Parent { get; set; }
    [Parameter] public object? Data { get; set; }
    [Parameter] public MemberInfo? MemberInfo { get; set; }
    [Parameter] public bool Loading { get; set; }

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


    private bool DataHasStringViewerAttribute()
    {
        if (MemberInfo != null)
        {
            var memberAttr = MemberInfo.GetCustomAttribute<TvStringAttribute>();

            if (memberAttr != null)
            {
                return true;
            }

            var memberType = GetMemberType(MemberInfo);

            var isMemberTypeString = memberType?.GetCustomAttribute<TvStringAttribute>() != null;

            if (isMemberTypeString)
            {
                return true;
            }
        }
        var dataType = Data?.GetType();
        if (dataType != null)
        {
            if (dataType.GetCustomAttribute<TvStringAttribute>() != null)
            {
                return true;
            }
        }

        return false;

        Type? GetMemberType(MemberInfo memberInfo)
        {
            return memberInfo switch
            {
                PropertyInfo propertyInfo => propertyInfo.PropertyType,
                FieldInfo fieldInfo => fieldInfo.FieldType,
                _ => null,
            };
        }
    }
}
