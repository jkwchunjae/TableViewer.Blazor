﻿namespace TableViewerBlazor;

public partial class TableEditor3<T,
    TParent1, TItem1,
    TParent2, TItem2,
    TParent3, TItem3
    >
    : TableEditor2<T,
        TParent1, TItem1,
        TParent2, TItem2
        > where T : class
{
    [Parameter] public RenderFragment<CustomEditorArgument<TParent3, TItem3>>? CustomEditor3 { get; set; }

    protected override void OnInitialized()
    {
        SetCustomEditor();
    }

    protected override void SetCustomEditor()
    {
        base.SetCustomEditor();
        if (CustomEditor3 != null)
        {
            if (Options.CustomEditorOptions.CustomEditor3 == null)
            {
                throw new Exception("Options.CustomEditorOptions.CustomEditor3 is null");
            }
            else
            {
                Options.CustomEditorOptions.CustomEditor3.RenderFragment = ConvertRenderFragment(CustomEditor3);
            }
        }
    }
}
