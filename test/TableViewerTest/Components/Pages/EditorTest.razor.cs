using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

public class EditorTestData
{
    [TeAutocomplete(nameof(state))]
    public string state { get; set; } = string.Empty;
    [TeAutocomplete(nameof(stateData))]
    public StateData stateData { get; set; } = new();
}

public class StateData
{
    public int Index { get; set; }
    public string Name { get; set; } = string.Empty;
}

[Route("/editor-test")]
public partial class EditorTest : ComponentBase
{
    [Inject] private IJSRuntime Js { get; set; } = default!;

    public EditorTestData data = new(); 
    private List<StateData> stateData { get; set; } = new();

    public TeOptions options = new TeOptions();

    private List<string> states = new List<string>
    {
        "Alabama", "Alaska", "American Samoa", "Arizona",
        "Arkansas", "California", "Colorado", "Connecticut",
        "Delaware", "District of Columbia", "Federated States of Micronesia",
        "Florida", "Georgia", "Guam", "Hawaii", "Idaho",
    };

    protected override void OnInitialized()
    {
        data = new EditorTestData
        {
            state = "California"
        };

        // 외부에서 받은 데이터
        stateData = new()
        {
            new StateData
            {
                Index = 0,
                Name = "Alabama"
            },
            new StateData
            {
                Index = 1,
                Name = "Alaska"
            },
            new StateData
            {
                Index = 2,
                Name = "California"
            },
            new StateData
            {
                Index = 3,
                Name = "Guam"
            }
        };

        options = new TeOptions
        {
            AutocompleteOptions =
            {
                new TeAutocompleteOption<string>
                {
                    Id = nameof(EditorTestData.state),
                    Items = states.Select(s => new TeAutocompleteItem<string>(s)).ToList(),
                    //StringConverter = obj => obj.Value
                },
                new TeAutocompleteOption<StateData>
                {
                    Id = nameof(EditorTestData.stateData),
                    Items = stateData.Select(d => new TeAutocompleteItem<StateData>(d)).ToList(),
                    StringConverter = value => $"{value.Value.Index} {value.Value.Name}",
                }
            }
        };
        base.OnInitialized();
    }


    private async Task OnDataChanged(object value)
    {
        await Js.InvokeVoidAsync("console.log", value);
    }
}
