using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using TableViewerBlazor.Helper;
using TableViewerBlazor.Public;

namespace TableViewerBlazor.Internal.TvComponent;

public partial class TvElementView : TvViewBase
{
    [Parameter] public object? Parent { get; set; }
    [Parameter] public object? Data { get; set; }
    [Parameter] public MemberInfo? MemberInfo { get; set; }

    private bool IsNumber => Data switch
    {
        byte => true,
        int => true,
        long => true,
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

        var attr = MemberInfo.GetCustomAttribute<TvStringViewerAttribute>();

        if (attr != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
