using TableViewer.MewUI.Internal.Viewing;
using TableViewer.MewUI.Options;
using Xunit;

namespace TableViewer.MewUI.Tests;

/// <summary>A01~A06: source-compatible action conditions, async lifecycle and link/popup data.</summary>
public sealed class ParityActionTests
{
    [Fact]
    public async Task GenericActionRejectsWrongTypesWithoutCallingUserCode()
    {
        var calls = 0;
        var conditions = 0;
        ITvAction action = new TvAction<int>
        {
            Condition = (value, depth) => { conditions++; return value == 7 && depth == 3; },
            Action = value => { calls += value; return Task.CompletedTask; }
        };
        Assert.False(action.Condition("7", 3));
        Assert.False(action.Condition(null, 3));
        Assert.Equal(0, conditions);
        Assert.True(action.Condition(7, 3));
        Assert.False(action.Condition(7, 2));
        await action.Action("wrong type");
        Assert.Equal(0, calls);
        await action.Action(7);
        Assert.Equal(7, calls);
    }

    [Fact]
    public async Task OneClickActionCannotRunTwiceAndReportsRunningAndDone()
    {
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var finish = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var calls = 0;
        using var controller = new TvActionController(new TvOneClickAction<int>
        {
            Action = async _ => { calls++; started.SetResult(); await finish.Task; }
        }, 7);
        var states = new List<TvActionState>();
        controller.Changed += () => states.Add(controller.State);
        var first = controller.InvokeAsync();
        await started.Task;
        Assert.Equal(TvActionState.Running, controller.State);
        await controller.InvokeAsync();
        Assert.Equal(1, calls);
        finish.SetResult();
        await first;
        Assert.Equal(TvActionState.Done, controller.State);
        await controller.InvokeAsync();
        Assert.Equal(1, calls);
        Assert.Equal([TvActionState.Running, TvActionState.Done], states);
    }

    [Fact]
    public async Task ReusableOneClickAndPlainActionsReturnToReady()
    {
        var calls = 0;
        using var oneClick = new TvActionController(new TvOneClickAction<int> { Reusable = true, Action = _ => { calls++; return Task.CompletedTask; } }, 1);
        await oneClick.InvokeAsync();
        await oneClick.InvokeAsync();
        Assert.Equal(2, calls);
        Assert.Equal(TvActionState.Ready, oneClick.State);
        using var plain = new TvActionController(new TvAction<int> { Action = _ => { calls++; return Task.CompletedTask; } }, 1);
        await plain.InvokeAsync();
        await plain.InvokeAsync();
        Assert.Equal(4, calls);
        Assert.Equal(TvActionState.Ready, plain.State);
    }

    [Fact]
    public async Task TwoStepActionArmsFirstAndExecutesOnlyOnSecondClickWithinWindow()
    {
        var calls = 0;
        using var controller = new TvActionController(new TvDoubleClickAction<int>
        {
            ResetDelay = TimeSpan.FromMinutes(1),
            Action = _ => { calls++; return Task.CompletedTask; }
        }, 1);
        await controller.InvokeAsync();
        Assert.Equal(TvActionState.Armed, controller.State);
        Assert.Equal(0, calls);
        await controller.InvokeAsync();
        Assert.Equal(1, calls);
        Assert.Equal(TvActionState.Ready, controller.State);
        await controller.InvokeAsync();
        Assert.Equal(TvActionState.Armed, controller.State);
        Assert.Equal(1, calls);
    }

    [Fact]
    public async Task ExpiredTwoStepWindowCannotExecuteAction()
    {
        var calls = 0;
        using var controller = new TvActionController(new TvDoubleClickAction<int>
        {
            ResetDelay = TimeSpan.Zero,
            Action = _ => { calls++; return Task.CompletedTask; }
        }, 1);
        await controller.InvokeAsync();
        await controller.InvokeAsync();
        Assert.Equal(0, calls);
        Assert.Equal(TvActionState.Ready, controller.State);
    }

    [Fact]
    public async Task FailedActionExposesErrorAndAllowsRetry()
    {
        var failure = new InvalidOperationException("operation failed");
        var shouldFail = true;
        using var controller = new TvActionController(new TvOneClickAction<int>
        {
            Action = _ => shouldFail ? Task.FromException(failure) : Task.CompletedTask
        }, 1);
        Assert.Same(failure, await Assert.ThrowsAsync<InvalidOperationException>(controller.InvokeAsync));
        Assert.Same(failure, controller.Error);
        Assert.Equal(TvActionState.Ready, controller.State);
        shouldFail = false;
        await controller.InvokeAsync();
        Assert.Equal(TvActionState.Done, controller.State);
        Assert.Null(controller.Error);
    }

    [Fact]
    public void LinksPopupContentAndTooltipRemainPerValueConfiguration()
    {
        ITvLink link = new TvLink<int> { Href = value => $"https://example.test/{value}", Target = "_blank", Condition = (value, depth) => value > 0 && depth == 2 };
        Assert.Equal("https://example.test/7", link.Href(7));
        Assert.Equal("", link.Href("wrong type"));
        Assert.True(link.Condition(7, 2));
        Assert.False(link.Condition(7, 1));
        Assert.Equal("_blank", link.Target);
        ITvPopupAction popup = new TvPopupAction<int>
        {
            Action = _ => Task.CompletedTask,
            PopupTitle = value => $"Delete {value}?",
            PopupContent = value => $"Selected {value}",
            InnerButtonOptions = new() { CloseLabel = "Cancel", ConfirmLabel = "Delete" },
            TooltipOption = new() { Text = "Delete this row" }
        };
        Assert.Equal("Delete 7?", popup.PopupTitle(7));
        Assert.Equal("Selected 7", popup.PopupContent(7));
        Assert.Equal("Cancel", popup.InnerButtonOptions.CloseLabel);
        Assert.Equal("Delete", popup.InnerButtonOptions.ConfirmLabel);
        Assert.Equal("Delete this row", popup.TooltipOption!.Text);
    }
}
