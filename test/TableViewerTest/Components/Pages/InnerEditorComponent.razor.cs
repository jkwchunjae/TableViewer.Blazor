using Microsoft.AspNetCore.Components;
using TableViewerBlazor.Internal.TeComponent;

namespace TableViewerTest.Components.Pages;

public partial class InnerEditorComponent : TeEditorBase
{
    private EditInner? _inner => (EditInner?)base.Data;
}
