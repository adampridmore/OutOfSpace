using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using static CompressLib.HexParser;
using static CompressLib.RunLengthEncoding;

namespace CompressLib;

public class RunLengthEncodingTests
{
    [Fact]
    public void RleText2()
    {
        var textToRle = "01 01 01";
        
        var expectedRleTest = "01 01 03";
        
        Assert.Equal(expectedRleTest, RleEncode2(textToRle));
    }

    [Fact]
    public void RleTextWithMoreThat256Repeat()
    {
        string textToRle =  String.Join("", Enumerable.Repeat("01 ", 510)).Trim();
        
        var expectedRleTest = "01 01 FF 01 01 FF";
        
        Assert.Equal(expectedRleTest, RleEncode2(textToRle));
    }
    
    [Fact]
    public void DecodeRleText2()
    {
        var textToRleDecode = "01 01 03";
        
        var expectedDecodedText = "01 01 01";
        
        Assert.Equal(expectedDecodedText, RleDecode2(textToRleDecode));
    }

    // [Fact]
    // public void SlidingWindow()
    // {
    //     var list = new []{1, 2, 3};
    //
    //     var results = new List<List<int>>();
    //     list.Window2(items =>
    //     {
    //         results.Add(items);
    //     });
    //
    //     var expected = new List<List<int>>
    //     {
    //         new() {1,2},
    //         new() {2,3},
    //     };
    //         
    //     Assert.Equal(expected, results);
    // }
    
    //TODO: RLE in a block size (e.g. 3 as pixels are 3 bytes
    //TODO: Use double character a RLE trigger
}