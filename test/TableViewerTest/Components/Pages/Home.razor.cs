﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.Json;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject] IJSRuntime Js { get; set; } = default!;

    TvOptions? arrayDataOption = null;
    TvOptions? optionForPeople = null;
    TvOptions? personOption = new TvOptions
    {
        GlobalOpenDepth = 1,
        StringTypes =
        {
            typeof(PersonName),
        },
    };
    TvOptions? optionForPR = null;

    #region Test data
    object arrayTestData = new object?[] { null, 2, null, 2, 3, };

    Person person = new Person
    {
        Id = 213,
        Name = new PersonName("User name"),
        Attributes = new Dictionary<string, object>
        {
            ["age"] = 12,
        },
        EnumValue = TestEnum.A,
    };

    Person[] people = new Person[]
    {
        new Person
        {
            Id = 213,
            Name = new PersonName("User name"),
            Birth = DateTime.UtcNow,
            Death = new DateTime(2300, 1, 11, 5, 12, 1, DateTimeKind.Utc),
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
                    Name: new PersonName("User name"),
                    Attributes: new Dictionary<string, object>
                    {
                        ["age"] = 12,
                        ["true"] = true,
                        ["false"] = false,
                    }
                ),
                new PersonRecord(
                    Id: 213,
                    Name: new PersonName("User 2"),
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
                    new PersonRecord(1, new PersonName("a"), new Dictionary<string, object>{ ["a"] = 1 }),
                    new PersonRecord(1, new PersonName("a"), new Dictionary<string, object>{ ["a"] = 1 }),
                    new PersonRecord(1, new PersonName("a"), new Dictionary<string, object>{ ["a"] = 1 }),
                },
            },
        },
        new Person
        {
            Id = 999,
            Name = new PersonName("second name"),
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
                    new PersonRecord(1, new PersonName("a"), new Dictionary<string, object>{ ["a"] = 1 }),
                    new PersonRecord(1, new PersonName("a"), new Dictionary<string, object>{ ["a"] = 1 }),
                    new PersonRecord(1, new PersonName("a"), new Dictionary<string, object>{ ["a"] = 1 }),
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
            Name: new PersonName("User name"),
            Attributes: new Dictionary<string, object>
            {
                ["age"] = 12,
                ["true"] = true,
                ["false"] = false,
            }
        ),
        new PersonRecord(
            Id: 999,
            Name: new PersonName("second name"),
            Attributes: new Dictionary<string, object>
            {
                ["age"] = 12,
                ["location"] = "France",
            }
        ),
    };
    #endregion

    protected override Task OnInitializedAsync()
    {
        // people = [];
        #region Options
        arrayDataOption = new TvOptions
        {
            Title = "배열 테스트",
            Actions =
            {
                new TvDoubleClickAction<int>
                {
                    Label = "Console log",
                    Condition = (number, depth) => true,
                    Action = async number =>
                    {
                        await Js.InvokeVoidAsync("console.log", number);
                    },
                    SecondStyle = new TvSecondButtonStyle
                    {
                        Variant = Variant.Filled,
                    },
                },
                new TvAction<int>
                {
                    Label = "Console log 2",
                    Action = async number =>
                    {
                        await Js.InvokeVoidAsync("console.log", number);
                    },
                },
            },
            Links =
            {
                new TvLink<int>
                {
                    Href = number => $"/update-{number}",
                    Label = "Update",
                },
            },
        };
        optionForPeople = new TvOptions
        {
            Title = "테이블 테스트",
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
                    Action = async (person) =>
                    {
                        await ConsoleWrite(person);
                    },
                    Condition = (person, depth) => person.Name == new PersonName("User 2"),
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
                        return data?.Name == new PersonName("User name");
                    },
                },
                // new TvYamlEditorOption<Dictionary<string, object>>(),
            },
            DateTime = new TvDateTimeOption
            {
                RelativeTime = true,
                //Format = "MM-dd-yyyy HH:mm:ss",
            },
        };

        optionForPR = new TvOptions
        {
            Title = "PersonRecord 테스트",
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
            Links =
            {
                new TvLink<PersonRecord>
                {
                    Href = p => $"/update-{p.Name}",
                    Label = "Update",
                },
            },
            // Editor =
            // {
            //     new TvJsonEditorOption<PersonRecord>(),
            // },
        };
        #endregion

        return Task.CompletedTask;
    }

    private async Task ConsoleWrite(object obj)
    {
        await Js.InvokeVoidAsync("console.log", obj);
    }
}
