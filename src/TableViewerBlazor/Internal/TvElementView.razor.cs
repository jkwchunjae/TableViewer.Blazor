using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace TableViewerBlazor.Internal;

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

    private bool IsObjectArray
    {
        get {
            // if (Data?.GetType()?.IsArray ?? false)
            // {
            //     return Data.GetType().GetElementType()?.IsClass ?? false;
            // }
            // if (Data?.GetType()?.GenericTypeArguments?.Length > 0)
            // {
            //     return Data!.GetType().GenericTypeArguments[0].IsClass;
            // }
            if (Data is IEnumerable arr)
            {
                var firstData = arr.Cast<object>().FirstOrDefault();
                if (firstData != null)
                {
                    return firstData!.GetType().IsClass;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
