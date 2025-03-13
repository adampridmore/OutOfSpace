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
}