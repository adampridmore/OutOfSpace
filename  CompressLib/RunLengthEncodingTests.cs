using static CompressLib.RunLengthEncoding;

namespace CompressLib;

public class RunLengthEncodingTests
{
    [Fact]
    public void RleText()
    {
        var textToRle = "01 02 03 01 02 03 01 02 03 01 02 03";
        
        var expectedRleTest = "01 02 03 01 02 03 00 00 04";
        
        Assert.Equal(expectedRleTest, RleEncode(textToRle));
    }
    
    [Fact]
    public void RleText_DontEncodeSingleBytes()
    {
        var textToRle = "01 02 03";
        
        var expectedRleTest = "01 02 03";
        
        Assert.Equal(expectedRleTest, RleEncode(textToRle));
    }
     
    [Fact]
    public void RleTextWithMoreThat256Repeat()
    {
        string textToRle =  String.Join("", Enumerable.Repeat("01 ", 510)).Trim();
        
        var expectedRleTest = "01 01 01 01 01 01 00 00 AA";
        
        Assert.Equal(expectedRleTest, RleEncode(textToRle));
    }
    
    [Fact]
    public void DecodeRleText()
    {
        var textToRleDecode = "01 01 01 01 01 01 01 01 03";
        
        var expectedDecodedText = "01 01 01 01 01 01 01 01 01";
        
        Assert.Equal(expectedDecodedText, RleDecode(textToRleDecode));
    }
}