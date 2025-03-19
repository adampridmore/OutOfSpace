using static CompressLib.HexParser;

namespace CompressLib;

public static class RunLengthEncoding
{
    public static Byte3[] RleDecode(Byte3[] byteToDecode)
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
        for (var i = 0; i < byteToDecode.Length; i++)
        {
            var firstByte = byteToDecode[i];
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

    public static string RleDecode(string hexToDecode)
    {
        var byteToDecode = HexToBytes(hexToDecode);

        return BytesToHex(RleDecode(byteToDecode.ToArray()));
    }

    public static Byte3[] RleEncode(Byte3[] inputBytes)
    {
        void WriteRleEncodedValue(Byte3? currentByte1, List<Byte3> outputBytes, int count)
        {
            if (currentByte1 == null)
            {
                return;
            }

            outputBytes.Add(currentByte1);
            if (count > 1)
            {
                outputBytes.Add(currentByte1);
                outputBytes.Add(new Byte3(0, 0, Convert.ToByte(count)));
            }
        }

        Byte3? currentByteRun = null;
        var currentByteCount = 0;
        var outputBytes = new List<Byte3>();

        foreach (var currentByte in inputBytes)
        {
            if (currentByte == currentByteRun && currentByteCount < 255)
            {
                currentByteCount++;
            }
            else
            {
                WriteRleEncodedValue(currentByteRun, outputBytes, currentByteCount);

                currentByteRun = currentByte;
                currentByteCount = 1;
            }
        }

        WriteRleEncodedValue(currentByteRun, outputBytes, currentByteCount);

        return outputBytes.ToArray();
    }

    public static string RleEncode(string hexString)
    {
        var inputBytes = HexToBytes(hexString).ToArray();
        return BytesToHex(RleEncode(inputBytes));
    }
}