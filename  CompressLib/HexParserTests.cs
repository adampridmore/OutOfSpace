using static CompressLib.HexParser;

namespace CompressLib;

public class HexParserTests
{
    private string _hex = "00 01 FF";
    
    [Fact]
    public void ParseHexString()
    {
        var hexBytes = HexToBytes(_hex).ToList();
        Assert.Equal(3, hexBytes.Count);
        
        Assert.Equal((byte)0x00, hexBytes[0]);
        Assert.Equal((byte)0x01, hexBytes[1]);
        Assert.Equal((byte)0xFF, hexBytes[2]);
    }

    [Fact]
    public void HexToStringText()
    {
        var bytes = new byte [] { 0x00, 0x01, 0xff };
        Assert.Equal("00 01 FF", BytesToHex(bytes));
    }

    [Fact]
    public void RleText()
    {
        var textToRle = "01 01 01";
        
        var expectedRleTest = "01 03";
        
        Assert.Equal(expectedRleTest, RleEncode(textToRle));
    }
    
    [Fact]
    public void RleTextWithMoreThat256Repeat()
    {
        string textToRle =  String.Join("", Enumerable.Repeat("01 ", 510)).Trim();
        
        var expectedRleTest = "01 FF 01 FF";
        
        Assert.Equal(expectedRleTest, RleEncode(textToRle));
    }
    
    [Fact]
    public void DecodeRleText()
    {
        var textToRleDecode = "01 03";
        
        var expectedDecodedText = "01 01 01";
        
        Assert.Equal(expectedDecodedText, RleDecode(textToRleDecode));
    }

    [Fact]
    public void CompressSif()
    {
        var imageBytes = File.ReadAllBytes(@"/Users/adampridmore/work/Dev/york-code-dojo/OutOfSpace/Images/_DSC2240_DxO.sif");
        var convertedBytes = RleEncode(imageBytes);

        var destinationFileRle = @"/Users/adampridmore/work/Dev/york-code-dojo/OutOfSpace/ImagesRle/_DSC2240_DxO.sif";
        File.WriteAllBytes(destinationFileRle, convertedBytes);

        var decodeBytes = RleDecode(convertedBytes);

        var destinationFileDecoded = @"/Users/adampridmore/work/Dev/york-code-dojo/OutOfSpace/ImagesRleDecompressed/_DSC2240_DxO.sif";

        File.WriteAllBytes(destinationFileDecoded, decodeBytes);
    }
}