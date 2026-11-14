using System.Reflection;
using TableViewer.Share.Options;
using TableViewer.Share.Public;
using Xunit;
using Blazor = TableViewerBlazor.Options;
using Native = TableViewer.MewUI.Options;

namespace TableViewer.Share.Tests;

public sealed class SharedContractTests
{
    [Fact]
    public void SharedAssemblyHasNoUiFrameworkDependency()
    {
        var references = typeof(TvStringAttribute).Assembly.GetReferencedAssemblies().Select(name => name.Name!).ToArray();
        Assert.DoesNotContain(references, name => name.Contains("MewUI") || name.Contains("MudBlazor") || name.Contains("AspNetCore"));
    }

    [Fact]
    public void ViewerPoliciesAreActuallyDeclaredInSharedAssembly()
    {
        foreach (var type in new[] { typeof(Blazor.TvOptions), typeof(Native.TvOptions) })
        {
            var policy = type.GetProperty(nameof(Native.TvOptions.ColumnVisible))!;
            Assert.Same(typeof(TvColumnVisibleOption).Assembly, policy.DeclaringType!.Assembly);
        }
    }

    [Fact]
    public void OneSerializedOptionCanBeUsedInBothViewers()
    {
        var serializer = new TvJsonEditorOption<Model>();
        var blazor = new Blazor.TvOptions { EditorOptions = [serializer] };
        var native = new Native.TvOptions { EditorOptions = [serializer] };
        var model = new Model();
        Assert.True(blazor.TryGetCustomOption(null, model, 1, "$", out var left));
        Assert.True(native.TryGetCustomOption(null, model, 1, "$", out var right));
        Assert.Same(serializer, left);
        Assert.Same(left, right);
    }

    [Fact]
    public void OneModelAttributeSelectsTheRightEditorInBothFrameworks()
    {
        var blazorField = new Blazor.TeTextFieldOption { Id = "shared-name" };
        var nativeField = new Native.TeTextFieldOption { Id = "shared-name" };
        var member = typeof(Model).GetProperty(nameof(Model.Name))!;
        var blazor = new Blazor.TeOptions { TextFieldOptions = [blazorField] };
        var native = new Native.TeOptions { TextFieldOptions = [nativeField] };
        Assert.True(Blazor.TeOptionsExtension.TryGetFieldOption(blazor, member,
            new BlazorEditorProbe("Ada", blazor), out var left));
        Assert.True(Native.TeOptionsExtension.TryGetFieldOption(native, member,
            new MewUI.Internal.Editing.TeEditorBase { Data = "Ada", Options = native }, out var right));
        Assert.Same(blazorField, left);
        Assert.Same(nativeField, right);
        Assert.IsType<TeTextFieldAttribute>(member.GetCustomAttribute<TeFieldAttribute>());
    }

    [Fact]
    public async Task OneValidatorAndConverterCanBeReusedAcrossBothEditors()
    {
        var validation = new TeValidation<string> { Func = value => value.Length >= 2, Message = "Too short" };
        var blazor = new Blazor.TeTextFieldOption { Validations = [validation] };
        var native = new Native.TeTextFieldOption { Validations = [validation] };
        Assert.Same(blazor.Validations[0], native.Validations[0]);
        Assert.False(await ((ITeValidation)validation).Func("A"));
        Assert.True(await ((ITeValidation)validation).Func("Ada"));
        var converter = new TeTextFieldConverter<int> { StringValue = number => $"#{number}", FromString = text => int.Parse(text.TrimStart('#')) };
        Assert.Equal(12, converter.FromField(converter.ToField(12)!));
        Assert.Same(converter, new Blazor.TeTextFieldOption<int> { Converter = converter }.Converter);
        Assert.Same(converter, new Native.TeTextFieldOption<int> { Converter = converter }.Converter);
    }

    [Fact]
    public void RuntimeAndDeclaredDefaultsKeepTheirSelectionPrecedence()
    {
        var generic = new Candidate("", typeof(string).FullName!);
        var builtIn = new Candidate("built-in", "");
        Assert.True(TeFieldOptionResolver.TryResolve(null, "Ada", [generic], _ => builtIn, _ => null, out var runtime));
        Assert.Same(generic, runtime);
        var member = typeof(Model).GetProperty(nameof(Model.Name))!;
        Assert.True(TeFieldOptionResolver.TryResolve(member, null, [generic], _ => null, _ => builtIn, out var declared));
        Assert.Same(builtIn, declared);
        Assert.True(TeFieldOptionResolver.TryResolve(member, null, [generic], _ => null, _ => null, out var fallback));
        Assert.Same(generic, fallback);
    }

    [Fact]
    public void CustomEditorAttributeWinsOverEarlierTypeCondition()
    {
        var conditional = new TeCustomEditorOption<Func<object>> { Condition = (_, _) => true };
        var attributed = new TeCustomEditorOption<Func<object>> { Id = "custom" };
        var selected = TeCustomEditorPolicy.Find(typeof(Model).GetProperty(nameof(Model.Custom)), typeof(int), new[] { conditional, attributed });
        Assert.Same(attributed, selected);
    }

    [Fact]
    public void BothActionAdaptersUseTheSharedDispatcherAndKeepTheirNativeStyles()
    {
        Assert.Equal(typeof(TvAction<,,>), typeof(Blazor.TvAction<int>).BaseType!.GetGenericTypeDefinition());
        Assert.Equal(typeof(TvAction<,,>), typeof(Native.TvAction<int>).BaseType!.GetGenericTypeDefinition());
        Assert.NotEqual(typeof(Blazor.TvButtonStyle), typeof(Native.TvButtonStyle));
    }

    public sealed class Model
    {
        [TeTextField("shared-name")] public string Name { get; set; } = "Ada";
        [TvString] public Version Version { get; set; } = new(1, 2, 3);
        [TeCustomEditor("custom")] public int Custom { get; set; }
    }

    private sealed record Candidate(string? Id, string TypeName) : ITeGenericTypeOption;

    private sealed class BlazorEditorProbe : TableViewerBlazor.Internal.TeComponent.TeEditorBase
    {
        public BlazorEditorProbe(object data, Blazor.TeOptions options) { Data = data; Options = options; }
    }
}
