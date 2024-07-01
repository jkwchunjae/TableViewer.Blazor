﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.Json;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject] IJSRuntime Js { get; set; } = default!;


    object test = new object?[] { null, 2, null, 2, 3, };
    TvOptions optionForTest = new TvOptions
    {
        Actions = new List<ITvAction>()
        {
            new TvDoubleClickAction<int>
            {
                Label = "Console log",
                Condition = (number, depth) => true,
                Action = number =>
                {
                    Console.WriteLine(number);
                    return Task.CompletedTask;
                },
                SecondStyle = new TvSecondButtonStyle
                {
                    Variant = Variant.Filled,
                },
            },
        },
    };
    TvOptions? optionForPeople = null;
    Person person = new Person
    {
        Id = 213,
        Name = "User name",
        Attributes = new Dictionary<string, object>
        {
            ["age"] = 12,
        },
        EnumValue = TestEnum.A,
    };
    TvOptions personOption = new TvOptions
    {
        GlobalOpenDepth = 1,
    };

    Person[] people = new Person[]
    {
        new Person
        {
            Id = 213,
            Name = "User name",
            Birth = DateTime.UtcNow,
            Attributes = new Dictionary<string, object>
            {
                ["age"] = 12,
                ["true"] = true,
                ["false"] = false,
            },
            People = new PersonRecord[]
            {
                new PersonRecord(
                    Id: 213,
                    Name: "User name",
                    Attributes: new Dictionary<string, object>
                    {
                        ["age"] = 12,
                        ["true"] = true,
                        ["false"] = false,
                    }
                ),
                new PersonRecord(
                    Id: 213,
                    Name: "User 2",
                    Attributes: new Dictionary<string, object>
                    {
                        ["age"] = 12,
                        ["true"] = true,
                        ["false"] = false,
                    }
                ),
            },
            OpenTest = new OpenTest
            {
                Id = "123",
                Numbers = new int[]
                {
                    1, 2, 3,
                },
                People = new PersonRecord[]
                {
                    new PersonRecord(1, "a", new Dictionary<string, object>{ ["a"] = 1 }),
                    new PersonRecord(1, "a", new Dictionary<string, object>{ ["a"] = 1 }),
                    new PersonRecord(1, "a", new Dictionary<string, object>{ ["a"] = 1 }),
                },
            },
        },
        new Person
        {
            Id = 999,
            Name = "second name",
            Birth = new DateTime(1989, 2, 1, 5, 12, 1, DateTimeKind.Utc),
            Attributes = new Dictionary<string, object>
            {
                ["age"] = 12,
                ["location"] = "France",
            },
            OpenTest = new OpenTest
            {
                Id = "123",
                Numbers = new int[]
                {
                    1, 2, 3,
                },
                People = new PersonRecord[]
                {
                    new PersonRecord(1, "a", new Dictionary<string, object>{ ["a"] = 1 }),
                    new PersonRecord(1, "a", new Dictionary<string, object>{ ["a"] = 1 }),
                    new PersonRecord(1, "a", new Dictionary<string, object>{ ["a"] = 1 }),
                },
            },
        },
    };

    PersonRecord[] peopleRecord = new PersonRecord[]
    {
        null,
        new PersonRecord(
            Id: 0,
            Name: null,
            Attributes: null
        ),
        new PersonRecord(
            Id: 213,
            Name: "User name",
            Attributes: new Dictionary<string, object>
            {
                ["age"] = 12,
                ["true"] = true,
                ["false"] = false,
            }
        ),
        new PersonRecord(
            Id: 999,
            Name: "second name",
            Attributes: new Dictionary<string, object>
            {
                ["age"] = 12,
                ["location"] = "France",
            }
        ),
    };

    TvOptions optionForPR = null;

    protected override Task OnInitializedAsync()
    {
        optionForPeople = new TvOptions
        {
            GlobalOpenDepth = 1,
            Actions = new List<ITvAction>()
            {
                new TvDoubleClickAction<Person>
                {
                    Condition = (person, depth) => true,
                    Label = "Console log person",
                    Action = async (person) =>
                    {
                        await ConsoleWrite(person);
                    },
                    ResetDelay = TimeSpan.FromSeconds(1),
                    Style = new TvButtonStyle
                    {
                        StartIcon = Icons.Material.Filled.Person,
                        IconSize = Size.Small,
                        SuperDense = false,
                    },
                    SecondStyle = new TvSecondButtonStyle
                    {
                        Variant = Variant.Filled,
                        Color = Color.Tertiary,
                        IconColor = Color.Dark,
                    },
                },
                new TvAction<PersonRecord>
                {
                    Label = "Console log person record",
                },
            },
            OpenDepth =
            {
                new TvOpenDepthOption<OpenTest>
                {
                    OpenDepth = 3,
                },
            },
            Editor =
            {
                new TvYamlEditorOption<PersonRecord>()
                {
                    Condition = (data, depth, path) =>
                    {
                        return data?.Name == "User name";
                    },
                },
                // new TvYamlEditorOption<Dictionary<string, object>>(),
            },
        };

        optionForPR = new TvOptions
        {
            Actions =
            {
                new TvAction<PersonRecord>
                {
                    Action = async (person) =>
                    {
                        await Js.InvokeVoidAsync("console.log", person);
                    },
                },
            },
            Editor =
            {
                new TvJsonEditorOption<PersonRecord>(),
            },
        };

        return Task.CompletedTask;
    }

    private async Task ConsoleWrite(object obj)
    {
        await Js.InvokeVoidAsync("console.log", obj);
    }
}
