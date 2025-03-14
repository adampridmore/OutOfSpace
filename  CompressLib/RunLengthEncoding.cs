using System.Globalization;
using static CompressLib.HexParser;

namespace CompressLib;

public static class RunLengthEncoding
{
    
    public static byte[] RleDecode(byte[] byteToDecode)
    {
        var decoded = byteToDecode
            .Chunk(2)
            .SelectMany(pair =>
            {
                var b = pair[0];
                var runCount = pair[1];
                return Enumerable.Repeat(b,runCount).ToList();
            })
            .ToArray();

        return decoded.ToArray();
    }
    
    public static string RleDecode(string hexToDecode)
    {
        var byteToDecode = HexToBytes(hexToDecode);

        return BytesToHex(RleDecode(byteToDecode.ToArray()));
    }

    public static byte[] RleEncode(byte[] inputBytes)
    {
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

        return outputBytes.ToArray();
    }
    
    public static string RleEncode(string hexString)
    {
        var inputBytes = HexToBytes(hexString).ToArray();
        return BytesToHex(RleEncode(inputBytes));
    }
}