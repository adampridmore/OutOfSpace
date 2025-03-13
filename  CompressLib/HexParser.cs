using System.Globalization;
using System.Runtime.InteropServices.JavaScript;

namespace CompressLib;

public class HexParser
{
    private string _hex = "00 01 FF";

    public static byte TextDigitToByte(string hexDigit)
    {
        return byte.Parse(hexDigit, NumberStyles.HexNumber);
    }
    
    public static string BytesToHex(byte [] bytes)
    {
        return BitConverter
            .ToString(bytes)
            .Replace("-", " ");
    }

    public IEnumerable<byte> HexToBytes(string hex)
    {
        return hex
            .Split(" ")
            .Select(TextDigitToByte)
            .ToArray();
    }
    
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

    private string RleDecode(string hexToDecode)
    {
        var byteToDecode = HexToBytes(hexToDecode);
        var outputBytes = new List<byte>();

        var decoded = byteToDecode
            .Chunk(2)
            .SelectMany(pair =>
            {
                var b = pair[0];
                var runCount = pair[1];
                return Enumerable.Repeat(b,runCount).ToList();
            })
            .ToArray();

        return BytesToHex(decoded);
    }
    
    
    private string RleEncode(string hexString)
    {
        var inputBytes = HexToBytes(hexString);
        byte? currentByte = null;
        var currentByteCount = 0;
        
        var outputBytes = new List<byte>();
        
        foreach (var b in inputBytes)
        {
            if (b == currentByte && currentByteCount < 255)
            {
                currentByteCount++;
            }
            else
            {
                if (currentByte.HasValue)
                {
                    outputBytes.Add(currentByte.Value);
                    outputBytes.Add(Convert.ToByte(currentByteCount));
                }
                
                currentByte = b;
                currentByteCount = 1;
            }
        }
        
        if (currentByte.HasValue)
        {
            outputBytes.Add(currentByte.Value);
            outputBytes.Add(Convert.ToByte(currentByteCount));
        }

        return BytesToHex(outputBytes.ToArray());
    }
}