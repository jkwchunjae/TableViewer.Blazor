using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using TableViewerBlazor.Public;

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
            if (elementType.GetCustomAttribute<TvStringAttribute>() != null)
            {
                return false;
            }
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
                if (nestedType.GetCustomAttribute<TvStringAttribute>() != null)
                {
                    return false;
                }
                if (nestedType.IsObject())
                {
                    return true;
                }
                var firstType = arr.Cast<object?>().FirstOrDefault()?.GetType();
                return firstType?.IsObject() ?? false;
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
        if (type == typeof(DateTime))
        {
            return false;
        }
        if (type.IsPrimitive)
        {
            return false;
        }
        if (CheckAnonymousType(type))
        {
            return true;
        }
        if (type == typeof(object))
        {
            return false;
        }
        return true;

        bool CheckAnonymousType(Type type)
        {
            // https://stackoverflow.com/questions/2483023/how-to-test-if-a-type-is-anonymous
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && type.Attributes.HasFlag(TypeAttributes.NotPublic);
        }
    }
}
