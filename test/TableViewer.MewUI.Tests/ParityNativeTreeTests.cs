using System.Collections;
using Aprillz.MewUI;
using Aprillz.MewUI.Controls;
using Aprillz.MewUI.MewvalonEdit;
using TableViewer.MewUI.Options;
using Xunit;
using MewSize = Aprillz.MewUI.Size;

namespace TableViewer.MewUI.Tests;

/// <summary>Checks the actual native control tree without claiming rendered-pixel or input coverage.</summary>
[Collection(NativeControlCollection.Name)]
public sealed class ParityNativeTreeTests
{
    [Fact]
    public void V07_ObjectsRenderTheirValuesInsideNativeNestedTables()
    {
        using var viewer = new global::TableViewer.MewUI.TableViewer(new { Name = "Ada", Detail = new { Role = "Engineer" } }, new() { GlobalOpenDepth = 3 });
        BuildVisualTree(viewer);
        var elements = Descendants(viewer).ToArray();
        var texts = TextValues(elements);
        Assert.Contains("Name", texts);
        Assert.Contains("Ada", texts);
        Assert.Contains("Detail", texts);
        Assert.Contains("Role", texts);
        Assert.Contains("Engineer", texts);
        Assert.True(elements.OfType<Grid>().Count() >= 2);
    }

    [Fact]
    public void V09_PrimitiveCollectionsInitiallyShowTwoItemsAndMoreButton()
    {
        using var viewer = new global::TableViewer.MewUI.TableViewer(new[] { "first", "second", "third", "fourth" });
        BuildVisualTree(viewer);
        var texts = TextValues(Descendants(viewer));
        Assert.Contains("first", texts);
        Assert.Contains("second", texts);
        Assert.DoesNotContain("third", texts);
        Assert.Contains("more", texts);
    }

    [Fact]
    public void V10_O02_DisabledDictionaryStringKeysDoNotLeakIntoRenderedValues()
    {
        using var viewer = new global::TableViewer.MewUI.TableViewer(new Hashtable { ["Name"] = "Ada", ["Secret"] = "private-value" }, new() { DisableKeys = ["Secret"] });
        BuildVisualTree(viewer);
        var texts = TextValues(Descendants(viewer));
        Assert.Contains("Ada", texts);
        Assert.DoesNotContain("Secret", texts);
        Assert.DoesNotContain("private-value", texts);
    }

    [Fact]
    public void O02_DisabledObjectMembersAreAbsentFromActualNativeTree()
    {
        using var viewer = new global::TableViewer.MewUI.TableViewer(new { Name = "Ada", Secret = "private-value" }, new() { DisableKeys = ["Secret"] });
        BuildVisualTree(viewer);
        var texts = TextValues(Descendants(viewer));
        Assert.Contains("Ada", texts);
        Assert.DoesNotContain("Secret", texts);
        Assert.DoesNotContain("private-value", texts);
    }

    [Theory]
    [InlineData("json", "Json")]
    [InlineData("yaml", "YAML")]
    public void C01_SerializedViewUsesReadOnlyNativeSyntaxEditorWithSizeLimits(string language, string expectedHighlighting)
    {
        ITvEditorOption option = language == "json"
            ? new TvJsonEditorOption<RecordFixture> { LayoutMaxSize = new(120, 260) }
            : new TvYamlEditorOption<RecordFixture> { LayoutMaxSize = new(120, 260) };
        using var viewer = new global::TableViewer.MewUI.TableViewer(new RecordFixture("Ada", 98), new() { EditorOptions = [option] });
        BuildVisualTree(viewer);
        var controls = Descendants(viewer).ToArray();
        var editors = controls.OfType<TextEditor>().ToArray();
        Assert.True(editors.Length == 1, string.Join(" | ", TextValues(controls)));
        var editor = editors[0];
        Assert.True(editor.IsReadOnly);
        Assert.False(editor.ShowLineNumbers);
        Assert.Contains("Ada", editor.Text);
        Assert.Equal(expectedHighlighting, editor.SyntaxHighlighting?.Name);
        Assert.InRange(editor.Height, 24, 120);
        Assert.InRange(editor.Width, 100, 260);
    }

    [Fact]
    public void V12_LoadingAndEmptyContentAppearInActualTree()
    {
        using var viewer = new global::TableViewer.MewUI.TableViewer(Array.Empty<string>(), new() { Contents = new() { LoadingContent = "Loading fixture", NoRecordsContent = "Empty fixture" } }) { Loading = true };
        BuildVisualTree(viewer);
        var elements = Descendants(viewer).ToArray();
        var texts = TextValues(elements);
        Assert.Contains("Loading fixture", texts);
        Assert.Contains("Empty fixture", texts);
        Assert.Contains(elements.OfType<ProgressBar>(), progress => progress.IsIndeterminate);
    }

    [Fact]
    public void E03_NativeTextFieldReceivesConfigureCallbackAndSourceValue()
    {
        Control? configured = null;
        var options = new TeOptions
        {
            TextFieldOptions = [new TeTextFieldOption { Id = "name", Property = new() { Configure = input => { configured = input; input.Tag = "configured-text"; } } }]
        };
        using var editor = new TableEditor<EditorFixture>(new() { Name = "Ada" }, options);
        BuildVisualTree(editor);
        var text = Assert.IsType<TextBox>(configured);
        Assert.Equal("Ada", text.Text);
        Assert.Equal("configured-text", text.Tag);
        Assert.Contains(text, Descendants(editor));
    }

    [Fact]
    public void A06_ObjectArrayKeepsOriginalRootTypeForTitleButtonsAndEachRowForConditions()
    {
        var values = new[] { new RecordFixture("Ada", 98), new RecordFixture("Grace", 99) };
        using var viewer = new global::TableViewer.MewUI.TableViewer(values, new()
        {
            TitleButtons = [new TvAction<RecordFixture[]> { Label = "Whole collection action", Action = _ => Task.CompletedTask }],
            Buttons = [new TvAction<RecordFixture>
            {
                Label = "Ada row action", Condition = (item, depth) => item.Name == "Ada" && depth == 1,
                Action = _ => Task.CompletedTask
            }]
        });
        BuildVisualTree(viewer);
        var texts = TextValues(Descendants(viewer));
        Assert.Single(texts, text => text == "Whole collection action");
        Assert.Single(texts, text => text == "Ada row action");
        Assert.Contains("Ada", texts);
        Assert.Contains("Grace", texts);
    }

    [Fact]
    public void O04_UseTableFalseChangesRootObjectIntoSectionsWhileNestedObjectRemainsTable()
    {
        using var viewer = new global::TableViewer.MewUI.TableViewer(new { Name = "Ada", Nested = new { City = "Seoul" } }, new()
        {
            GlobalOpenDepth = 3,
            Style = new() { UseTable = false }
        });
        BuildVisualTree(viewer);
        var controls = Descendants(viewer).ToArray();
        Assert.Single(controls.OfType<Grid>());
        var texts = TextValues(controls);
        Assert.Contains("Name", texts);
        Assert.Contains("Nested", texts);
        Assert.Contains("City", texts);
        Assert.Contains("Seoul", texts);
    }

    public sealed record RecordFixture(string Name, int Score);
    public sealed class EditorFixture
    {
        [TeTextField("name")]
        public string Name { get; set; } = "";
    }

    private static string[] TextValues(IEnumerable<Element> elements) => elements.Select(element => element switch
    {
        TextBlock text => text.Text,
        Label label => label.Text,
        _ => null
    }).OfType<string>().ToArray();

    private static void BuildVisualTree(Element root)
    {
        // UserControl builds lazily at the start of its first measure. This test intentionally
        // stops at the first font measurement, which requires the app's graphics backend.
        // It verifies the composed logical tree; it does not claim headless layout coverage.
        try { root.Measure(new MewSize(1200, 800)); }
        catch (InvalidOperationException exception) when (exception.Message.StartsWith("No graphics backend registered.", StringComparison.Ordinal)) { }
    }

    private static IEnumerable<Element> Descendants(Element root)
    {
        yield return root;
        if (root is not ILogicalTreeHost host) yield break;
        var children = new List<Element>();
        host.VisitLogicalChildren(child => { children.Add(child); return true; });
        foreach (var child in children)
            foreach (var element in Descendants(child)) yield return element;
    }
}
