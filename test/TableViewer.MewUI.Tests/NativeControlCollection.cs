using Xunit;

namespace TableViewer.MewUI.Tests;

// Native control construction shares framework style/font caches and must have the same
// single-thread ownership as a real MewUI dispatcher. Data-only parity tests can stay parallel.
[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class NativeControlCollection
{
    public const string Name = "MewUI native controls";
}
