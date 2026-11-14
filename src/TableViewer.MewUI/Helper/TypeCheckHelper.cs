using TableViewer.MewUI.Internal.ViewerModel;

namespace TableViewer.MewUI.Helper;

public static class TypeCheckHelper
{
    public static bool IsObjectArray(this object? data) => ViewerData.IsObjectArray(data);
    public static bool IsObject(this Type type) => ViewerData.IsObject(type);
}
