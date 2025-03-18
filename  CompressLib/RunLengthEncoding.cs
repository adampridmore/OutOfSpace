using static CompressLib.HexParser;

namespace CompressLib;

public static class RunLengthEncoding
{
    public static Byte3[] RleDecode2(Byte3[] byteToDecode)
    {
        Byte3? TryGetByte3(Byte3[] bytes, int index)
        {
            if (index >= bytes.Length)
            {
                return null;
            }
            
            return bytes[index];
        }
        
        var decodedBytes = new List<Byte3>();
        for (var i = 0 ; i < byteToDecode.Length; i++)
        {
            var firstByte =  byteToDecode[i];
            var secondByte = TryGetByte3(byteToDecode, i + 1);
            var thirdByte = TryGetByte3(byteToDecode, i + 2);
            
            if (firstByte == secondByte)
            {
                var b = firstByte;
                var runCount = thirdByte.ToInt();
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
    
    public static Byte3[] RleEncode2(Byte3[] inputBytes)
    {
        void WriteRleEncodedValue(Byte3? currentByte1, List<Byte3> bytes, int count)
        {
            if (currentByte1 == null)
            {
                return;
            }

            if (count == 1)
            {
                bytes.Add(currentByte1);
            }
            else
            {
                bytes.Add(currentByte1);
                bytes.Add(currentByte1);
                bytes.Add(new Byte3(0, 0, Convert.ToByte(count)));
            }
        }

        Byte3? currentByte = null;
        var currentByteCount = 0;
        
        var outputBytes = new List<Byte3>();
        
        foreach (var b in inputBytes)
        {
            if (b == currentByte && currentByteCount < 255)
            {
                currentByteCount++;
            }
            else
            {
                if (currentByte!=null)
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