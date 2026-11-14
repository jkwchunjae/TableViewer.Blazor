using TableViewer.MewUI.Options.Property;
using Xunit;

namespace TableViewer.MewUI.Tests;

public sealed class TextMaskTests
{
    [Theory]
    [InlineData("01012345678", "010-1234-5678")]
    [InlineData("010-1234-5678", "010-1234-5678")]
    public void PatternAcceptsRawAndFormattedText(string input, string expected) =>
        Assert.Equal(expected, new TePatternMask("000-0000-0000").Apply(input));

    [Fact]
    public void PatternAllowsPartialTypingButRejectsInvalidCommittedText()
    {
        var mask = new TePatternMask("000-0000");
        Assert.Equal("01", mask.FormatInput("01"));
        Assert.Throws<FormatException>(() => mask.Apply("01"));
        Assert.Throws<FormatException>(() => mask.FormatInput("0x"));
        Assert.Throws<FormatException>(() => mask.Apply("12345678"));
        Assert.Equal("", mask.Apply(""));
    }

    [Fact]
    public void PatternSupportsLettersAlphanumericAndEscapedLiterals()
    {
        Assert.Equal("ab-12", new TePatternMask("aa-**").Apply("ab12"));
        Assert.Equal("0-42", new TePatternMask(@"\0-00").Apply("42"));
    }

    [Fact]
    public void BlocksAcceptRawAndFormattedValues()
    {
        var mask = new TeBlockMask { Separator = ":", Blocks = [new(2, new TePatternMask("00")), new(2, new TePatternMask("00"))] };
        Assert.Equal("12:34", mask.Apply("1234"));
        Assert.Equal("12:34", mask.Apply("12:34"));
        Assert.Equal("12:3", mask.FormatInput("123"));
        Assert.Throws<FormatException>(() => mask.Apply("123"));
        Assert.Throws<FormatException>(() => mask.Apply("12345"));
    }

    [Fact]
    public void MultiMaskChoosesByPrefixAndUsesFallback()
    {
        var mask = new TeMultiMask
        {
            Choices = [new(text => text.StartsWith('+'), new TePatternMask("+00-0000"))],
            Fallback = new TePatternMask("000-0000")
        };
        Assert.Equal("123-4567", mask.Apply("1234567"));
        Assert.Equal("+82-1234", mask.Apply("+821234"));
        Assert.Equal("+82-1", mask.FormatInput("+821"));
    }
}
