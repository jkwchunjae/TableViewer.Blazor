﻿using Microsoft.AspNetCore.Components;
using TableViewerBlazor.Options;

namespace TableViewerBlazor.Internal;

public class TvViewBase : ComponentBase
{
    [Parameter] public TvOptions? Options { get; set; }
    [Parameter] public int Depth { get; set; }
}
