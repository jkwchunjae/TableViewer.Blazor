using TableViewerBlazor.Helper;

namespace UnitTest;

public class PrimitiveArrayTest
{
    [Fact]
    public void PrimitiveArrayTest1()
    {
        var arr = new object?[] { 1, 2, 3, 4, 5 };

        Assert.False(arr.IsObjectArray());
    }
    [Fact]
    public void PrimitiveArrayTest2()
    {
        var arr = new int[] { 1, 2, 3, 4, 5 };

        Assert.False(arr.IsObjectArray());
    }
    [Fact]
    public void PrimitiveArrayTest3()
    {
        var arr = new object?[] { null, 2, 3, 4, 5 };

        Assert.False(arr.IsObjectArray());
    }
    [Fact]
    public void PrimitiveArrayTest4()
    {
        var arr = new string[] { "1", "2", "3", "4", "5" };

        Assert.False(arr.IsObjectArray());
    }
    [Fact]
    public void PrimitiveArrayTest5()
    {
        var arr = new object?[] { 1, 2, null, 4, 5, "6" };

        Assert.False(arr.IsObjectArray());
    }
}