using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTest")]
namespace TableViewerBlazor.Helper;

internal static class TypeCheckHelper
{
    public static bool IsObjectArray(this object? data)
    {
        if (data == null)
            return false;

        var dataType = data.GetType();

        if (dataType.IsArray)
        {
            var arr = (data as IEnumerable)!.Cast<object>();
            var elementType = dataType.GetElementType()!;
            if (elementType.IsObject())
            {
                return true;
            }
            var firstType = arr.FirstOrDefault()?.GetType();
            return firstType?.IsObject() ?? false;
        }
        else if (data is IEnumerable arr)
        {
            if (dataType.GenericTypeArguments.Length == 1)
            {
                var nestedType = dataType.GenericTypeArguments[0];
                return nestedType.IsObject();
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public static bool IsObject(this Type type)
    {
        if (type == typeof(string))
        {
            return false;
        }
        if (type.IsPrimitive)
        {
            return false;
        }
        if (type == typeof(object))
        {
            return false;
        }
        return true;
    }
}
