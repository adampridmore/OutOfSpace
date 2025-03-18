using static CompressLib.HexParser;
using static CompressLib.RunLengthEncoding;

namespace CompressLib;

public class RunLengthEncodingFileTests
{
    private string _imagesFolder = @"/Users/adampridmore/work/Dev/york-code-dojo/OutOfSpace/Images";
    private string _imagesRleFolder = @"/Users/adampridmore/work/Dev/york-code-dojo/OutOfSpace/ImagesRle";
    private string _imagesRleDecompressedFolder = @"/Users/adampridmore/work/Dev/york-code-dojo/OutOfSpace/ImagesRleDecompressed";

    // [Fact(Skip = "In progress")]
    [Fact]
    public void CompressSif()
    {
        var imageBytes = File.ReadAllBytes(Path.Combine(_imagesFolder,"_DSC2240_DxO.sif"));
        
        var convertedBytes = RleEncode2(Byte3.ToByte3(imageBytes));

        var destinationFileRle = Path.Combine(_imagesRleFolder, "_DSC2240_DxO.sif");
        File.WriteAllBytes(destinationFileRle, Byte3.ToRawBytes(convertedBytes));

        var decodeBytes = RleDecode2(convertedBytes);

        var destinationFileDecoded = Path.Combine(_imagesRleDecompressedFolder, "_DSC2240_DxO.sif");

        File.WriteAllBytes(destinationFileDecoded, Byte3.ToRawBytes(decodeBytes));
    }
    
    [Fact]
    public void RleAllFiles()
    {
        Directory.Delete(_imagesRleFolder, true);
        Directory.CreateDirectory(_imagesRleFolder);
        
        Directory.Delete(_imagesRleDecompressedFolder, true);
        Directory.CreateDirectory(_imagesRleDecompressedFolder);

        foreach (var filename in Directory.GetFiles(_imagesFolder))
        {
            var bytes = File.ReadAllBytes(filename);
            var rleBytes = RleEncode2(Byte3.ToByte3(bytes));

            var destinationFile = Path.Combine(_imagesRleFolder, Path.GetFileName(filename));
            File.WriteAllBytes(destinationFile, Byte3.ToRawBytes(rleBytes));
        }
        
        foreach (var filename in Directory.GetFiles(_imagesRleFolder))
        {
            var bytes = File.ReadAllBytes(filename);
            var rleBytes = RleDecode2(Byte3.ToByte3(bytes));

            var destinationFile = Path.Combine(_imagesRleDecompressedFolder, Path.GetFileName(filename));
            File.WriteAllBytes(destinationFile, Byte3.ToRawBytes(rleBytes));
        }
    }
    
    //TODO: RLE in a block size (e.g. 3 as pixels are 3 bytes
    //TODO: Use double character a RLE trigger
}