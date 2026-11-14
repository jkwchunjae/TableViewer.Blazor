using System.Collections;
using TableViewer.MewUI.Internal.Editing;
using TableViewer.MewUI.Options;
using TableViewer.MewUI.Options.Property;
using Xunit;

namespace TableViewer.MewUI.Tests;

public sealed class EditorPortTests
{
    [Fact]
    public async Task AttributeIdWinsAndConvertsBackIntoSourceValue()
    {
        var data = new Model { Count = 3 };
        var options = new TeOptions
        {
            TextFieldOptions = [new TeTextFieldOption<int>
            {
                Id = "count-text",
                Converter = new() { StringValue = value => $"#{value}", FromString = value => int.Parse(value.TrimStart('#')) }
            }],
            NumericFieldOptions = [new TeNumericFieldOption<int>()]
        };
        var session = new TeEditSession(data, options);
        var node = session.Nodes.Single(node => node.Path == nameof(Model.Count));
        Assert.IsType<TeTextFieldOption<int>>(node.Option);
        Assert.Equal("#3", node.FieldValue);
        await session.SetFieldValueAsync(node, "#42");
        Assert.Equal(42, data.Count);
    }

    [Fact]
    public async Task ValidationUsesConvertedDomainValueAndRunsSequentially()
    {
        var order = new List<int>();
        var data = new Model();
        var options = new TeOptions { TextFieldOptions = [new TeTextFieldOption<int>
        {
            Id = "count-text",
            Converter = new() { StringValue = value => value.ToString(), FromString = int.Parse },
            Validations = [new TeAsyncValidation<int> { Func = async value => { order.Add(1); await Task.Yield(); order.Add(2); return value >= 10; }, Message = "At least ten" },
                new TeValidation<int> { Func = value => { order.Add(3); return value % 2 == 0; }, Message = "Even only" }]
        }] };
        var session = new TeEditSession(data, options);
        var node = session.Nodes.Single(node => node.Path == nameof(Model.Count));
        await session.SetFieldValueAsync(node, "3");
        Assert.Equal([1, 2, 3], order);
        Assert.Equal(["At least ten", "Even only"], node.Errors);
        Assert.Equal(3, data.Count);
    }

    [Fact]
    public async Task ConversionFailurePreservesSourceAndSurvivesValidate()
    {
        var session = new TeEditSession(new Model { Count = 12 }, new TeOptions { TextFieldOptions = [new TeTextFieldOption<int>
        { Id = "count-text", Converter = new() { StringValue = value => value.ToString(), FromString = int.Parse } }] });
        var node = session.Nodes.Single(node => node.Path == nameof(Model.Count));
        await session.SetFieldValueAsync(node, "invalid");
        Assert.Equal(12, ((Model)session.Data!).Count);
        Assert.NotEmpty(node.Errors);
        await session.ValidateAsync();
        Assert.NotEmpty(node.Errors);
        await session.SetFieldValueAsync(node, "20");
        Assert.Empty(node.Errors);
    }

    [Fact]
    public async Task WritesPublicFieldsAndNestedValuesButIgnoresReadOnlyAndIgnoredMembers()
    {
        var data = new Model { Address = new Address { City = "Seoul" } };
        var session = new TeEditSession(data);
        await session.SetValueAsync(session.Nodes.Single(node => node.Path == "Address.City"), "Busan");
        await session.SetValueAsync(session.Nodes.Single(node => node.Path == "PublicField"), "changed");
        Assert.Equal("Busan", data.Address.City);
        Assert.Equal("changed", data.PublicField);
        Assert.DoesNotContain(session.Nodes, node => node.Path == "Ignored" || node.Path == "ReadOnly");
    }

    [Fact]
    public async Task ConvertedListSupportsAddRemoveAndReplacement()
    {
        var data = new WithList { Items = ["one", "two"] };
        var listOption = TeListEditorOption<string>.Create("new");
        listOption.RemoveItemAction = new TvAction<object> { Action = _ => throw new InvalidOperationException("Source remove action is presentation only") };
        var options = new TeOptions { ListEditorOptions = [listOption] };
        var session = new TeEditSession(data, options);
        var list = session.Nodes.Single(node => node.Path == "Items");
        await session.SetFieldValueAsync(list.Children[0], "first");
        Assert.Equal("first", data.Items[0]);
        await session.AddItemAsync(list);
        Assert.Equal(["first", "two", "new"], data.Items);
        list = session.Nodes.Single(node => node.Path == "Items");
        await session.RemoveItemAsync(list, 1);
        Assert.Equal(["first", "new"], data.Items);
    }

    [Fact]
    public async Task ObjectListEditsNotifyCustomEditorsInTheSameRow()
    {
        var data = new List<Address> { new() { City = "Seoul", Country = "Korea" } };
        var session = new TeEditSession(data, new TeOptions { ObjectListEditorOptions = [new TeObjectListEditorOption<Address>()] });
        var city = session.Nodes.Single(node => node.Path == "[0].City");
        var country = session.Nodes.Single(node => node.Path == "[0].Country");
        var argument = session.CustomArgument(country);
        var notifications = 0;
        argument.ParentChanged += (_, _) => notifications++;
        await session.SetFieldValueAsync(city, "Busan");
        Assert.Equal("Busan", data[0].City);
        Assert.Equal(1, notifications);
    }

    [Fact]
    public async Task RequiredAndReadOnlyOptionsHaveBehavior()
    {
        var data = new Address { City = "" };
        var options = new TeOptions { TextFieldOptions = [new TeTextFieldOption { Id = "city", Property = new() { Required = true, RequiredError = "City required", ReadOnly = true } }] };
        var session = new TeEditSession(data, options);
        var node = session.Nodes.Single(node => node.Path == "City");
        await session.ValidateAsync();
        Assert.Contains("City required", node.Errors);
        await session.SetFieldValueAsync(node, "Cannot write");
        Assert.Equal("", data.City);
    }

    [Fact]
    public void NumericOperationsPreservePrecisionBoundsAndSteps()
    {
        var integer = new TeNumericFieldOption<ulong>();
        Assert.Equal(ulong.MaxValue, TeNumericOperations.Parse("18446744073709551615", integer));
        Assert.Equal(ulong.MaxValue, TeNumericOperations.Step(ulong.MaxValue, integer, 1));
        var number = new TeNumericFieldOption<decimal> { Property = new() { Min = -1m, Max = 1m, Step = .1m } };
        Assert.Equal(.1234567890123456789012345678m, TeNumericOperations.Parse("0.1234567890123456789012345678", number));
        Assert.Equal(.3m, TeNumericOperations.Step(.2m, number, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TeNumericOperations.Parse("2", number));
    }

    [Fact]
    public async Task GenericBooleanConverterAndCallbacksUseDomainValues()
    {
        var data = new WithBoolean { State = "off" };
        var options = new TeOptions { CheckBoxOptions = [new TeCheckBoxOption<string>
        { Id = "state", Converter = new() { ToBoolean = value => value == "on", FromBoolean = value => value ? "on" : "off" } }] };
        var session = new TeEditSession(data, options);
        var changed = 0;
        session.DataChanged = value => { Assert.Same(data, value); changed++; return Task.CompletedTask; };
        await session.SetFieldValueAsync(session.Nodes.Single(node => node.Path == "State"), true);
        Assert.Equal("on", data.State);
        Assert.Equal(1, changed);
    }

    [Fact]
    public void AllFiveTypedCustomSlotsAdaptValuesAndParentNotifications()
    {
        var options = new TeOptions { CustomEditorOptions = new()
        {
            CustomEditor1 = new() { Id = "one" }, CustomEditor2 = new() { Id = "two" }, CustomEditor3 = new() { Id = "three" },
            CustomEditor4 = new() { Id = "four" }, CustomEditor5 = new() { Id = "five" }
        } };
        var received = new List<string>();
        var notifications = 0;
        Aprillz.MewUI.Controls.Element Factory(CustomEditorArgument<Model, string> argument)
        {
            received.Add(argument.Value ?? "null");
            argument.ParentChanged += (_, _) => notifications++;
            return new Aprillz.MewUI.Controls.TextBlock();
        }
        var editor = new FiveSlotEditor { Options = options, CustomEditor1 = Factory, CustomEditor2 = Factory, CustomEditor3 = Factory, CustomEditor4 = Factory, CustomEditor5 = Factory };
        editor.ConfigureSlots();
        var parent = new Model();
        var argument = new CustomEditorArgument<Model, string> { Parent = parent, Value = "original" };
        foreach (var option in options.CustomEditorOptions.CustomEditors) option.RenderFragment!(argument);
        Assert.Equal(5, received.Count);
        argument.OnParentChanged(parent);
        Assert.Equal(5, notifications);
        editor.Dispose();
        argument.OnParentChanged(parent);
        Assert.Equal(5, notifications);
    }

    [Fact]
    public async Task CustomEditorArgumentsTrackRepeatedEditsWithoutRebuildingTheView()
    {
        var data = new Model { Count = 1 };
        var options = new TeOptions { CustomEditorOptions = new() { CustomEditor1 = new() { Condition = (member, _) => member?.Name == nameof(Model.Count) } } };
        CustomEditorArgument<Model, int>? typedArgument = null;
        var editor = new CounterSlotEditor
        {
            Data = data, Options = options,
            CustomEditor1 = argument => { typedArgument = argument; return new Aprillz.MewUI.Controls.TextBlock(); }
        };
        editor.ConfigureSlots();
        var node = editor.EditSession.Nodes.Single(node => node.Path == nameof(Model.Count));
        var argument = editor.EditSession.CustomArgument(node);
        options.CustomEditorOptions.CustomEditor1.RenderFragment!(argument);
        await typedArgument!.DataChanged(typedArgument.Value + 1);
        await typedArgument.DataChanged(typedArgument.Value + 1);
        Assert.Equal(3, data.Count);
        Assert.Equal(3, typedArgument.Value);
        Assert.Equal(3, argument.Value);
        editor.Dispose();
    }

    [Fact]
    public void NativeDefaultGlyphsDistinguishBooleanAndPopupStates()
    {
        var checkbox = new TeCheckBoxProperty();
        Assert.Equal(3, new[] { checkbox.CheckedIcon, checkbox.UncheckedIcon, checkbox.IndeterminateIcon }.Distinct().Count());
        var select = new TeSelectBoxProperty();
        Assert.NotEqual(select.OpenIcon, select.CloseIcon);
        Assert.Equal("×", select.ClearIcon);
        Assert.Equal("×", new TeTextFieldProperty().ClearIcon);
        Assert.Equal("×", new TeNumericFieldProperty<int>().ClearIcon);
    }

    private sealed class Model
    {
        [TeTextField("count-text")] public int Count { get; set; }
        public Address Address { get; set; } = new();
        public string PublicField = "field";
        [TeIgnore] public string Ignored { get; set; } = "ignore";
        public string ReadOnly => "read only";
    }
    private sealed class Address
    {
        [TeTextField("city")] public string City { get; set; } = "";
        public string Country { get; set; } = "";
    }
    private sealed class WithList { public List<string> Items { get; set; } = []; }
    private sealed class WithBoolean { [TeCheckBox("state")] public string State { get; set; } = "off"; }
    private sealed class FiveSlotEditor : TableEditor5<Model, Model, string, Model, string, Model, string, Model, string, Model, string>
    {
        public void ConfigureSlots() => SetCustomEditor();
    }
    private sealed class CounterSlotEditor : TableEditor1<Model, Model, int> { public void ConfigureSlots() => SetCustomEditor(); }
}
