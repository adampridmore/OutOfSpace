using System.Globalization;
using static CompressLib.HexParser;

namespace CompressLib;

public static class RunLengthEncoding
{
    public static byte[] RleDecode2(byte[] byteToDecode)
    {
        var decodedBytes = new List<byte>();
        for (var i = 0 ; i < byteToDecode.Length; i++)
        {
            var firstByte =  byteToDecode[i];
            var secondByte = byteToDecode[i + 1];
            var thirdByte = byteToDecode[i + 2];
            
            if (firstByte == secondByte)
            {
                var b = firstByte;
                var runCount = thirdByte;
                decodedBytes.AddRange(Enumerable.Repeat(b, runCount).ToList());
                i += 2;
            }
            else
            {
                decodedBytes.AddRange(firstByte);
            }
        }
        
        return decodedBytes.ToArray();
    }
    
    public static string RleDecode2(string hexToDecode)
    {
        var byteToDecode = HexToBytes(hexToDecode);

        return BytesToHex(RleDecode2(byteToDecode.ToArray()));
    }
    
    public static byte[] RleEncode2(byte[] inputBytes)
    {
        void WriteRleEncodedValue(byte? currentByte1, List<byte> bytes, int i)
        {
            if (!currentByte1.HasValue)
            {
                return;
            }

            bytes.Add(currentByte1.Value);
            bytes.Add(currentByte1.Value);
            bytes.Add(Convert.ToByte(i));
        }

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
                    WriteRleEncodedValue(currentByte, outputBytes, currentByteCount);
                }
                
                currentByte = b;
                currentByteCount = 1;
            }
        }
        
        WriteRleEncodedValue(currentByte, outputBytes, currentByteCount);

        return outputBytes.ToArray();
    }
    
    public static string RleEncode2(string hexString)
    {
        var inputBytes = HexToBytes(hexString).ToArray();
        return BytesToHex(RleEncode2(inputBytes));
    }
}