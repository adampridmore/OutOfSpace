using System.Globalization;

namespace CompressLib;

public static class HexParser
{
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

    public static IEnumerable<byte> HexToBytes(string hex)
    {
        return hex
            .Split(" ")
            .Select(TextDigitToByte)
            .ToArray();
    }
    
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