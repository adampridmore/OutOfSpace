using System.Globalization;

namespace CompressLib;

public static class HexParser
{
    public static byte TextDigitToByte(string hexDigit)
    {
        return byte.Parse(hexDigit, NumberStyles.HexNumber);
    }
    
    public static string BytesToHex(Byte3[] bytes)
    {
        return BitConverter
            .ToString(Byte3.ToRawBytes(bytes))
            .Replace("-", " ");
    }

    public static IEnumerable<Byte3> HexToBytes(string hex)
    {
        return hex
            .Split(" ")
            .Select(TextDigitToByte)
            .Chunk(3)
            .Select(a => new Byte3(a[0], a[1], a[2]))
            .ToArray();
    }
}