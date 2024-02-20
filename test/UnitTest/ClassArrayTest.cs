using TableViewerBlazor.Helper;

namespace UnitTest;

public class ClassArrayTest
{
    [Fact]
    public void ClassArrayTest1()
    {
        var arr = new object?[] { new Data(), new Data(), new Data() };

        Assert.True(arr.IsObjectArray());
    }
    [Fact]
    public void ClassArrayTest2()
    {
        var arr = new Data[] { new Data(), new Data(), new Data() };

        Assert.True(arr.IsObjectArray());
    }
    [Fact]
    public void ClassArrayTest3()
    {
        var arr = new Data[] {};

        Assert.True(arr.IsObjectArray());
    }
    [Fact]
    public void AnonymousArrayTest1()
    {
        IEnumerable<object> arr = new object[] { new { A = 1, B = string.Empty } };

        Assert.True(arr.IsObjectArray());
    }
    [Fact]
    public void AnonymousArrayTest2()
    {
        var arr = new Data[] { new Data(), new Data(), new Data() };
        IEnumerable<object> arr2 = arr.Select((x, i) => new { Index = i, Data = x });

        Assert.True(arr2.IsObjectArray());
    }
}

public class Data
{

}