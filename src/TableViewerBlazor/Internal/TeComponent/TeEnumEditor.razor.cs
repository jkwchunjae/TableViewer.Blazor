namespace TableViewerBlazor.Internal.TeComponent;

public partial class TeEnumEditor : TeEditorBase
{
    TeSelectBoxOption<object>? selectBoxOption;
    TeRadioOption<object>? radioOption;
    protected override void OnInitialized()
    {
        var values = Enum.GetValues(Data!.GetType());

        if (values.Length <= 3)
        {
            radioOption = new TeRadioOption<object>
            {
                Items = values
                    .Cast<Enum>()
                    .Select(e => new TeRadioItem<object>(e, e.ToString()))
                    .ToList(),
            };
        }
        else
        {
            selectBoxOption = new TeSelectBoxOption<object>
            {
                Items = Enum.GetValues(Data!.GetType())
                    .Cast<Enum>()
                    .Select(e => new TeSelectItem<object>(e, e.ToString()))
                    .ToList(),
            };
        }
    }
}
