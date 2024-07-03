using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using TableViewerBlazor.Helper;

namespace TableViewerBlazor.Internal.TvComponent;

public partial class TvElementView : TvViewBase
{
    [Parameter] public object? Parent { get; set; }
    [Parameter] public object? Data { get; set; }

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

    private TvDateTimeOption? GetDateTimeOption()
    {
        if (Data is DateTime dateTimeData && Parent != null)
        {
            var propertyName = Parent.GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(DateTime?))
                .Where(p => p.GetValue(Parent) is DateTime dateTime && dateTime == dateTimeData)
                .Select(p => p.Name)
                .FirstOrDefault();

            return Options.DateTime
                .Where(option => option.Condition(propertyName))
                .FirstOrDefault();
        }
        return null;
    }
}
