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
}

public class Data
{

}