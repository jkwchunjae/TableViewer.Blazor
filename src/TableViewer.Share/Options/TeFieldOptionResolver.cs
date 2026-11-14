using System.Reflection;

namespace TableViewer.Share.Options;

/// <summary>Applies the common attribute, runtime type, built-in default and declared type precedence.</summary>
public static class TeFieldOptionResolver
{
    public static bool TryResolve(
        MemberInfo? member,
        object? data,
        IEnumerable<ITeFieldOption> options,
        Func<object, ITeFieldOption?> runtimeDefault,
        Func<Type, ITeFieldOption?> declaredDefault,
        out ITeFieldOption? result)
    {
        var candidates = options.ToArray();
        var id = member?.GetCustomAttribute<TeFieldAttribute>()?.Id;
        if (id is not null)
        {
            result = candidates.FirstOrDefault(option => !string.IsNullOrEmpty(option.Id) && option.Id == id);
            if (result is not null) return true;
        }

        if (data is not null)
        {
            var type = data.GetType();
            result = candidates.OfType<ITeGenericTypeOption>()
                .FirstOrDefault(option => option.TypeName == type.FullName || option.TypeName == type.BaseType?.FullName);
            result ??= runtimeDefault(data);
            if (result is not null) return true;
        }

        var declaredType = member switch
        {
            PropertyInfo property => property.PropertyType,
            FieldInfo field => field.FieldType,
            null => null,
            _ => throw new ArgumentException("Expected a property or field.", nameof(member))
        };
        if (declaredType is not null)
        {
            result = declaredDefault(declaredType)
                ?? candidates.OfType<ITeGenericTypeOption>().FirstOrDefault(option => option.TypeName == declaredType.FullName);
            if (result is not null) return true;
        }
        result = null;
        return false;
    }
}
