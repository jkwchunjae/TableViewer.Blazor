using System.Collections;
using System.Reflection;
using System.Text.Json;
using TableViewer.MewUI.Helper;
using TableViewer.MewUI.Internal.ViewerModel;
using TableViewer.MewUI.Options;
using TableViewer.Share.Public;
using TableViewer.MewUI.Service;
using Xunit;

namespace TableViewer.MewUI.Tests;

/// <summary>Ports the original UnitTest fixtures and exercises documented V/O/C contracts.</summary>
public sealed class ParityViewerTests
{
    public static IEnumerable<object?[]> OriginalArrayFixtures()
    {
        // ClassArrayTest: object[], class[], empty class[], anonymous[], LINQ projection.
        yield return [new object?[] { new Item(), new Item(), new Item() }, true];
        yield return [new Item[] { new(), new(), new() }, true];
        yield return [Array.Empty<Item>(), true];
        yield return [new object[] { new { A = 1, B = "" } }, true];
        yield return [new Item[] { new(), new(), new() }.Select((data, index) => new { Index = index, Data = data }), true];
        // ClassEnumerableTest: List and HashSet, including statically known empty lists.
        yield return [new List<object?> { new Item(), new Item(), new Item() }, true];
        yield return [new List<Item> { new(), new(), new() }, true];
        yield return [new List<Item>(), true];
        yield return [new HashSet<object?> { new Item(), new Item(), new Item() }, true];
        yield return [new HashSet<Item> { new(), new(), new() }, true];
        // PrimitiveArrayTest: boxed, typed, null first, strings, and mixed primitive values.
        yield return [new object?[] { 1, 2, 3, 4, 5 }, false];
        yield return [new int[] { 1, 2, 3, 4, 5 }, false];
        yield return [new object?[] { null, 2, 3, 4, 5 }, false];
        yield return [new string[] { "1", "2", "3", "4", "5" }, false];
        yield return [new object?[] { 1, 2, null, 4, 5, "6" }, false];
    }

    [Theory]
    [MemberData(nameof(OriginalArrayFixtures))]
    public void OriginalCollectionFixturesRetainTheirClassification(object? data, bool expected)
    {
        Assert.Equal(expected, data.IsObjectArray());
    }

    [Fact]
    public void V03_AllThirteenNumericTypesUseScalarRenderer()
    {
        object[] values = [(sbyte)-1, (byte)1, (short)-2, (ushort)2, -3, 3u, -4L, 4UL, (nint)(-5), (nuint)5, 1.25f, 2.5d, 3.75m];
        Assert.All(values, value => Assert.Equal(ViewerValueKind.Number, Classify(value)));
    }

    [Fact]
    public void V01_V10_TypeDispatchKeepsScalarAndStructuredValuesDistinct()
    {
        Assert.Equal(ViewerValueKind.Null, Classify(null));
        Assert.Equal(ViewerValueKind.String, Classify(""));
        Assert.Equal(ViewerValueKind.Boolean, Classify(false));
        Assert.Equal(ViewerValueKind.Enum, Classify(DayOfWeek.Friday));
        Assert.Equal(ViewerValueKind.DateTime, Classify(new DateTime(2026, 11, 15)));
        Assert.Equal(ViewerValueKind.Object, Classify(new Item()));
        Assert.Equal(ViewerValueKind.Array, Classify(new[] { 1, 2 }));
        Assert.Equal(ViewerValueKind.ObjectArray, Classify(new[] { new Item() }));
        Assert.Equal(ViewerValueKind.Dictionary, Classify(new Hashtable { ["Name"] = "Ada", [1] = null }));
    }

    [Fact]
    public void V11_StringAttributeWorksOnMemberDeclaredTypeAndRuntimeType()
    {
        var parent = new AttributeMembers();
        Assert.Equal(ViewerValueKind.String, ViewerData.Classify(parent, parent.Member, Member<AttributeMembers>(nameof(AttributeMembers.Member)), 2, ".Member", new()).Kind);
        Assert.Equal(ViewerValueKind.String, ViewerData.Classify(parent, parent.Typed, Member<AttributeMembers>(nameof(AttributeMembers.Typed)), 2, ".Typed", new()).Kind);
        Assert.Equal(ViewerValueKind.String, ViewerData.Classify(parent, parent.Runtime, Member<AttributeMembers>(nameof(AttributeMembers.Runtime)), 2, ".Runtime", new()).Kind);
        Assert.False(new[] { new DisplayString("Ada") }.IsObjectArray());
        Assert.True(new[] { new RecordItem(1, "Ada") }.IsObjectArray());
        Assert.True(new[] { new StructItem { Number = 2 } }.IsObjectArray());
    }

    [Fact]
    public void V07_O02_ReadablePropertiesAndPublicFieldsAreFilteredByFirstMatchingColumnRule()
    {
        var data = new MembersFixture();
        var defaults = ViewerData.Members(data, new());
        Assert.Equal([nameof(MembersFixture.First), nameof(MembersFixture.Second), nameof(MembersFixture.Hidden), nameof(MembersFixture.PublicField)], defaults.Select(member => member.Name));
        var options = new TvOptions
        {
            ColumnVisible =
            [
                new() { Before = ["Missing"], After = [nameof(MembersFixture.Hidden)] },
                new() { Before = [nameof(MembersFixture.First), nameof(MembersFixture.Second)], After = [nameof(MembersFixture.PublicField), nameof(MembersFixture.Second), nameof(MembersFixture.First)] },
                new() { Before = [nameof(MembersFixture.First)], After = [nameof(MembersFixture.Hidden)] }
            ],
            DisableKeys = [nameof(MembersFixture.Second)]
        };
        var members = ViewerData.Members(data, options);
        // The source MemberInfo overload filters; it does not reorder members by After.
        Assert.Equal([nameof(MembersFixture.First), nameof(MembersFixture.PublicField)], members.Select(member => member.Name));
        Assert.Equal(["first", "field"], members.Select(member => ViewerData.GetValue(data, member)));
    }

    [Fact]
    public void V08_ColumnsUseFirstNonNullObjectAndReadHeterogeneousRowsByName()
    {
        object?[] data = [null, new { Name = "Ada", Score = 98 }, new { Name = "Grace", Active = true }];
        var columns = ViewerData.Columns(data, new());
        Assert.Equal(["Name", "Score"], columns.Select(member => member.Name));
        Assert.Equal("Grace", ViewerData.GetValue(data[2], columns[0]));
        Assert.Null(ViewerData.GetValue(data[2], columns[1]));
        Assert.Null(ViewerData.GetValue(null, columns[0]));
    }

    [Fact]
    public void O01_ConditionalDepthReceivesRealDataDepthAndPathAndOverridesInheritedDepth()
    {
        var target = new Item();
        var options = new TvOptions
        {
            OpenDepth =
            [
                new TvOpenDepthOption<Item> { OpenDepth = 8, Condition = (_, _, _) => false },
                new TvOpenDepthOption<Item> { OpenDepth = 3, Condition = (value, depth, path) => ReferenceEquals(value, target) && depth == 4 && path == ".People[1]" }
            ]
        };
        Assert.Equal(6, ViewerData.ResolveOpenDepth(target, 4, ".People[1]", 1, options));
        Assert.Equal(1, ViewerData.ResolveOpenDepth(target, 4, ".Other", 1, options));
        Assert.Equal(1, ViewerData.ResolveOpenDepth("wrong type", 4, ".People[1]", 1, options));
    }

    [Fact]
    public void C01_C03_CustomViewSelectionPreservesPriorityAndTypedConditions()
    {
        var parent = new Item();
        var editor = new TvEditorOption<string> { Condition = (value, depth, path) => value == "Ada" && depth == 2 && path == ".Name", Serializer = value => "custom:" + value };
        var link = new TvStringLinkOption<Item, string> { Href = (_, value) => "https://example.test/" + value };
        var image = new TvImageViewerOption<Item, string> { ImageUrl = (_, value) => value };
        var options = new TvOptions { EditorOptions = [editor], StringLinkOptions = [link], ImageViewerOptions = [image] };
        var classified = ViewerData.Classify(parent, "Ada", null, 2, ".Name", options);
        Assert.Equal(ViewerValueKind.Serialized, classified.Kind);
        Assert.Same(editor, classified.CustomOption);
        Assert.Equal("custom:Ada", ((ITvEditorOption)editor).Serializer!("Ada"));
        Assert.Equal(ViewerValueKind.StringLink, ViewerData.Classify(parent, "Ada", null, 2, ".Other", options).Kind);
        options.StringLinkOptions.Clear();
        Assert.Equal(ViewerValueKind.Image, ViewerData.Classify(parent, "Ada", null, 2, ".Other", options).Kind);
        Assert.Equal(ViewerValueKind.String, ViewerData.Classify(new object(), "Ada", null, 2, ".Other", options).Kind);
        Assert.Equal(ViewerValueKind.Null, ViewerData.Classify(parent, null, null, 2, ".Name", options).Kind);
    }

    [Fact]
    public void C01_JsonYamlAndCustomSerializersKeepStructuredContentAndLanguage()
    {
        var value = new RecordItem(1, "Ada");
        ITvEditorOption json = new TvJsonEditorOption<RecordItem> { LayoutMaxSize = new(200, 400) };
        using var document = JsonDocument.Parse(json.Serializer!(value));
        Assert.Equal("Ada", document.RootElement.GetProperty("Name").GetString());
        Assert.Equal("json", json.Language);
        Assert.Equal(new EditorSize(200, 400), json.LayoutMaxSize);
        ITvEditorOption yaml = new TvYamlEditorOption<RecordItem>();
        Assert.Contains("Name: Ada", yaml.Serializer!(value));
        Assert.Equal("yaml", yaml.Language); // Corrects the original json-language typo.
        var display = new DisplayString("Grace");
        ITvEditorOption displayYaml = new TvYamlEditorOption<DisplayString>();
        Assert.Equal("Grace", displayYaml.Serializer!(display).Trim());
        var settings = new JsonSerializerOptions { Converters = { new TvStringJsonConverter<DisplayString>() } };
        Assert.Equal("\"Grace\"", JsonSerializer.Serialize(display, settings));
    }

    [Fact]
    public void V06_DateFormattingUsesMinuteOffsetCultureAndInjectedRelativeClock()
    {
        var time = new DateTimeOffset(2026, 11, 15, 0, 0, 0, TimeSpan.Zero);
        var service = new DateTimeService { Options = new() { Offset = -330, Locale = "en-US" }, Clock = () => time };
        Assert.Equal("05:30 +05:30", service.Format(time, new() { Format = "HH:mm zzz" }));
        Assert.Equal("2 hours ago", service.Format(time.AddHours(-2), new() { RelativeTime = true }));
        Assert.Equal("in 2 hours", service.Format(time.AddHours(2), new() { RelativeTime = true }));
    }

    private static ViewerValueKind Classify(object? value) => ViewerData.Classify(null, value, null, 1, "", new()).Kind;
    private static MemberInfo Member<T>(string name) => typeof(T).GetMember(name).Single();
    public sealed class Item;
    public sealed record RecordItem(int Id, string Name);
    public struct StructItem { public int Number; }
    [TvString]
    public sealed record DisplayString(string Value) { public override string ToString() => Value; }
    public sealed class AttributeMembers
    {
        [TvString] public Item Member { get; } = new();
        public DisplayString Typed { get; } = new("typed");
        public object Runtime { get; } = new DisplayString("runtime");
    }
    public sealed class MembersFixture
    {
        public string First { get; } = "first";
        public string Second { get; } = "second";
        public string Hidden { get; } = "secret";
        public Type ExcludedType => typeof(string);
        public string this[int index] => index.ToString();
        public static string Static => "static";
        public string PublicField = "field";
    }
}
