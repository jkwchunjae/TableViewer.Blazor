using System.Collections;
using System.Reflection;
using TableViewer.MewUI.Options;
using TableViewer.MewUI.Public;

namespace TableViewer.MewUI.Internal.ViewerModel;

public enum ViewerValueKind { Null, Number, Enum, Boolean, DateTime, String, Dictionary, Array, ObjectArray, Object, Serialized, StringLink, Image }
public sealed record ViewerClassification(ViewerValueKind Kind, ITvCustomOption? CustomOption = null);
public sealed record ViewerReadError(string Message);

/// <summary>The source component's type dispatch and member selection, independent of native controls.</summary>
public static class ViewerData
{
    public static ViewerClassification Classify(object? parent, object? data, MemberInfo? member, int depth, string path, TvOptions options)
    {
        if (data is null) return new(ViewerValueKind.Null);
        if (options.TryGetCustomOption(parent, data, depth, path, out var custom))
            return new(custom switch
            {
                ITvEditorOption => ViewerValueKind.Serialized,
                ITvStringLinkOption => ViewerValueKind.StringLink,
                ITvImageViewerOption => ViewerValueKind.Image,
                _ => ViewerValueKind.Object
            }, custom);
        if (HasStringAttribute(data, member)) return new(ViewerValueKind.String);
        if (IsNumber(data)) return new(ViewerValueKind.Number);
        if (data is Enum) return new(ViewerValueKind.Enum);
        if (data is bool) return new(ViewerValueKind.Boolean);
        if (data is DateTime or DateTimeOffset) return new(ViewerValueKind.DateTime);
        if (data is string or char or Guid or Uri or TimeSpan or DateOnly or TimeOnly or ViewerReadError) return new(ViewerValueKind.String);
        if (data is IDictionary) return new(ViewerValueKind.Dictionary);
        if (IsObjectArray(data)) return new(ViewerValueKind.ObjectArray);
        if (data is IEnumerable) return new(ViewerValueKind.Array);
        return new(ViewerValueKind.Object);
    }

    public static bool HasStringAttribute(object data, MemberInfo? member = null) =>
        member?.GetCustomAttribute<TvStringAttribute>() is not null
        || MemberType(member)?.GetCustomAttribute<TvStringAttribute>() is not null
        || data.GetType().GetCustomAttribute<TvStringAttribute>() is not null;

    public static bool IsNumber(object? data) => data is sbyte or byte or short or ushort or int or uint or long or ulong or nint or nuint or float or double or decimal;

    public static bool IsObjectArray(object? data)
    {
        if (data is null or string or IDictionary || data is not IEnumerable enumerable) return false;
        var type = data.GetType();
        var elementType = type.IsArray ? type.GetElementType() : type.GetInterfaces().Append(type)
            .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))?.GenericTypeArguments[0];
        if (elementType is not null && IsObject(elementType)) return true;
        var first = enumerable.Cast<object?>().FirstOrDefault(value => value is not null);
        return first is not null && IsObject(first.GetType());
    }

    public static bool IsObject(Type type) => type != typeof(object) && type != typeof(string) && type != typeof(decimal)
        && type != typeof(DateTime) && type != typeof(DateTimeOffset) && type != typeof(Guid) && type != typeof(Uri)
        && type != typeof(TimeSpan) && type != typeof(DateOnly) && type != typeof(TimeOnly)
        && !type.IsPrimitive && !type.IsEnum && !typeof(IEnumerable).IsAssignableFrom(type)
        && type.GetCustomAttribute<TvStringAttribute>() is null;

    public static IReadOnlyList<MemberInfo> Members(object data, TvOptions options)
    {
        IEnumerable<MemberInfo> members = data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.GetMethod?.IsPublic == true && property.GetIndexParameters().Length == 0 && property.PropertyType != typeof(Type))
            .Cast<MemberInfo>()
            .Concat(data.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public).Where(field => field.FieldType != typeof(Type)));
        var all = members.ToArray();
        var visible = options.ColumnVisible.FirstOrDefault(option => option.Matched(all, member => member.Name));
        if (visible is not null) members = visible.NewKeys(all, member => member.Name);
        return members.Where(member => !options.DisableKeys.Contains(member.Name)).ToArray();
    }

    public static object? GetValue(object? data, MemberInfo member)
    {
        if (data is null) return null;
        try
        {
            // Object arrays may contain compatible but different runtime types.
            var actual = member.DeclaringType?.IsInstanceOfType(data) == true ? member
                : (MemberInfo?)data.GetType().GetProperty(member.Name) ?? data.GetType().GetField(member.Name);
            return actual switch
            {
                PropertyInfo property when property.GetIndexParameters().Length == 0 => property.GetValue(data),
                FieldInfo field => field.GetValue(data),
                _ => null
            };
        }
        catch (Exception exception)
        {
            return new ViewerReadError((exception is TargetInvocationException { InnerException: { } inner } ? inner : exception).Message);
        }
    }

    public static Type? MemberType(MemberInfo? member) => member switch
    {
        PropertyInfo property => property.PropertyType,
        FieldInfo field => field.FieldType,
        _ => null
    };

    public static IReadOnlyList<object?> Items(IEnumerable data) => data.Cast<object?>().ToArray();
    public static IReadOnlyList<MemberInfo> Columns(IEnumerable<object?> items, TvOptions options) =>
        items.FirstOrDefault(item => item is not null) is { } first ? Members(first, options) : [];

    public static int ResolveOpenDepth(object? data, int depth, string path, int inherited, TvOptions options)
    {
        var selected = options.OpenDepth.FirstOrDefault(option => option.Condition?.Invoke(data, depth, path) == true);
        return selected is null ? inherited : selected.OpenDepth + depth - 1;
    }
}
