using static CompressLib.HexParser;

namespace CompressLib;

public class HexParserTests
{
    private string _hex = "00 01 FF";
    
    [Fact]
    public void ParseHexString()
    {
        var hexBytes = Byte3.ToRawBytes(HexToBytes(_hex).ToList());
        Assert.Equal(3, hexBytes.Length);
        
        Assert.Equal((byte)0x00, hexBytes[0]);
        Assert.Equal((byte)0x01, hexBytes[1]);
        Assert.Equal((byte)0xFF, hexBytes[2]);
    }

    [Fact]
    public void HexToStringText()
    {
        var bytes = new byte [] { 0x00, 0x01, 0xff };
        Assert.Equal("00 01 FF", BytesToHex(Byte3.ToByte3(bytes)));
    }
}