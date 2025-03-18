namespace CompressLib;

public record Byte3(byte A, byte B, byte C)
{
    public int ToInt()
    {
        return C;
    }

    public static byte[] ToRawBytes(IEnumerable<Byte3> bytes3List)
    {
        return bytes3List
            .SelectMany(b => new List<byte> { b.A, b.B, b.C })
            .ToArray();
    }

    public static Byte3[] ToByte3(IEnumerable<byte> rawBytes)
    {
        return rawBytes
            .Chunk(3)
                .Select(x => new Byte3(x[0], x[1], x[2]))
                .ToArray();
    }
}