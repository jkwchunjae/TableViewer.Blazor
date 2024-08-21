using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

public class EditorTestData
{
    [TeAutocomplete(nameof(states))]
    public string[] states { get; set; } =
    {
        "Alabama", "Alaska", "American Samoa", "Arizona",
        "Arkansas", "California", "Colorado", "Connecticut",
        "Delaware", "District of Columbia", "Federated States of Micronesia",
        "Florida", "Georgia", "Guam", "Hawaii", "Idaho",
    };
    [TeAutocomplete(nameof(countryData))]
    public List<CountryData> countryData { get; set; } = new();
}

public class CountryData
{
    public int Index { get; set; }
    public string Name { get; set; } = string.Empty;
    
}

[Route("/editor-test")]
public partial class EditorTest : ComponentBase
{
    [Inject] private IJSRuntime Js { get; set; } = default!;

    public EditorTestData data { get; set; } = new EditorTestData()
    {
        countryData =
        {
            new CountryData
            {
                Index = 0,
                Name = "Alabama"
            },
            new CountryData
            {
                Index = 1,
                Name = "Alaska"
            },
            new CountryData
            {
                Index = 2,
                Name = "California"
            },
            new CountryData
            {
                Index = 3,
                Name = "Guam"
            }
        }
    };

    TeOptions options = new TeOptions
    {
        AutocompleteOptions =
        {
            new TeAutocompleteOption<EditorTestData>
            {
                Id = nameof(EditorTestData.states),
            },
            new TeAutocompleteOption<CountryData>
            {
                Id = nameof(EditorTestData.countryData),
                StringConverter = value => $"{value.Index} {value.Name}"
            }
        }
    };

    private async Task OnDataChanged(object value)
    {
        await Js.InvokeVoidAsync("console.log", value);
    }

}
