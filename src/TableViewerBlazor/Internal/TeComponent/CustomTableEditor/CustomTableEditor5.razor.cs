﻿namespace TableViewerBlazor.Internal.TeComponent.CustomTableEditor;

public partial class CustomTableEditor5<T,
    TParent1, TItem1,
    TParent2, TItem2,
    TParent3, TItem3,
    TParent4, TItem4,
    TParent5, TItem5
    >
    : CustomTableEditor4<T,
        TParent1, TItem1,
        TParent2, TItem2,
        TParent3, TItem3,
        TParent4, TItem4
        > where T : class
{
    [Parameter] public RenderFragment<CustomEditorTypedArgument<TParent5, TItem5>>? CustomEditor5 { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        SetCustomEditor();
    }

    private void SetCustomEditor()
    {
        if (CustomEditor5 != null)
        {
            if (Options.CustomEditorOptions.CustomEditor5 == null)
            {
                throw new Exception("Options.CustomEditorOptions.CustomEditor5 is null");
            }
            else
            {
                Options.CustomEditorOptions.CustomEditor5.RenderFragment = ConvertRenderFragment(CustomEditor5);
            }
        }
    }
}