using System.Collections;
using Aprillz.MewUI;
using Aprillz.MewUI.Controls;
using TableViewer.MewUI.Internal.ViewerModel;
using TableViewer.MewUI.Options;
using Xunit;

namespace TableViewer.MewUI.Tests;

[Collection(NativeControlCollection.Name)]
public class ViewerDictionaryTests
{
    [Fact]
    public void GenericDictionaryUsesDictionaryEnumeratorAndFiltersStringKeys()
    {
        IDictionary data = new Dictionary<string, object?> { ["Name"] = "Ada", ["Secret"] = "private-value" };
        var entries = ViewerData.Entries(data, new() { DisableKeys = ["Secret"] });
        Assert.Single(entries);
        Assert.Equal("Name", entries[0].Key);
        Assert.Equal("Ada", entries[0].Value);
    }

    [Fact]
    public void IntegerDictionaryKeysRemainTypedValues()
    {
        IDictionary data = new Dictionary<int, string> { [5] = "Five" };
        var entry = Assert.Single(ViewerData.Entries(data, new()));
        Assert.Equal(5, entry.Key);
        Assert.Equal("Five", entry.Value);
    }

    [Fact]
    public void NativeGenericDictionaryTreeDoesNotReplaceTheTableWithACastError()
    {
        using var viewer = new global::TableViewer.MewUI.TableViewer(new Dictionary<string, object?> { ["Name"] = "Ada", ["Secret"] = "private-value" }, new() { DisableKeys = ["Secret"] });
        try { viewer.Measure(new Aprillz.MewUI.Size(800, 600)); }
        catch (InvalidOperationException exception) when (exception.Message.StartsWith("No graphics backend registered.", StringComparison.Ordinal)) { }
        var texts = Descendants(viewer).OfType<TextBlock>().Select(text => text.Text).ToArray();
        Assert.Contains("Name", texts);
        Assert.Contains("Ada", texts);
        Assert.DoesNotContain("Secret", texts);
        Assert.DoesNotContain(texts, text => text.StartsWith("Error:", StringComparison.Ordinal));
    }

    private static IEnumerable<Element> Descendants(Element element)
    {
        yield return element;
        if (element is not ILogicalTreeHost host) yield break;
        var children = new List<Element>();
        host.VisitLogicalChildren(child => { children.Add(child); return true; });
        foreach (var child in children)
            foreach (var item in Descendants(child)) yield return item;
    }
}
