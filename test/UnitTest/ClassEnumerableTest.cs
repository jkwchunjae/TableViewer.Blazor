using TableViewerBlazor.Helper;

namespace UnitTest;

public class ClassEnumerableTest
{
    [Fact]
    public void ClassEnumerableTest1()
    {
        var arr = new List<object?> { new Data(), new Data(), new Data() };

        Assert.True(arr.IsObjectArray());
    }
    [Fact]
    public void ClassEnumerableTest2()
    {
        var arr = new List<Data> { new Data(), new Data(), new Data() };

        Assert.True(arr.IsObjectArray());
    }
    [Fact]
    public void ClassEnumerableTest3()
    {
        var arr = new List<Data> { };

        Assert.True(arr.IsObjectArray());
    }
    [Fact]
    public void ClassEnumerableTest4()
    {
        var arr = new HashSet<object?> { new Data(), new Data(), new Data() };

        Assert.True(arr.IsObjectArray());
    }
    [Fact]
    public void ClassEnumerableTest5()
    {
        var arr = new HashSet<Data> { new Data(), new Data(), new Data() };

        Assert.True(arr.IsObjectArray());
    }
}