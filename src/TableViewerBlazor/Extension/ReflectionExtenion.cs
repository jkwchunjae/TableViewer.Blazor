namespace TableViewerBlazor.Extension;

internal static class ReflectionExtenion
{
    public static Type MemberType(this MemberInfo member)
    {
        return member switch
        {
            PropertyInfo propertyInfo => propertyInfo.PropertyType,
            FieldInfo fieldInfo => fieldInfo.FieldType,
            _ => throw new NotSupportedException(),
        };
    }
}
