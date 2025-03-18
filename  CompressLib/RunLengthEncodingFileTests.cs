using static CompressLib.HexParser;
using static CompressLib.RunLengthEncoding;

namespace CompressLib;

public class RunLengthEncodingFileTests
{
    private string imagesFolder = @"/Users/adampridmore/work/Dev/york-code-dojo/OutOfSpace/Images";
    private string imagesRleFolder = @"/Users/adampridmore/work/Dev/york-code-dojo/OutOfSpace/ImagesRle";
    private string imagesRleDecompressedFolder = @"/Users/adampridmore/work/Dev/york-code-dojo/OutOfSpace/ImagesRleDecompressed";

    // [Fact(Skip = "In progress")]
    [Fact]
    public void CompressSif()
    {
        var imageBytes = File.ReadAllBytes(Path.Combine(imagesFolder,"_DSC2240_DxO.sif"));
        var convertedBytes = RleEncode2(imageBytes);

        var destinationFileRle = Path.Combine(imagesRleFolder, "_DSC2240_DxO.sif");
        File.WriteAllBytes(destinationFileRle, convertedBytes);

        var decodeBytes = RleDecode2(convertedBytes);

        var destinationFileDecoded = Path.Combine(imagesRleDecompressedFolder, "_DSC2240_DxO.sif");
        
        File.WriteAllBytes(destinationFileDecoded, decodeBytes);
    }
    
    [Fact]
    public void RleAllFiles()
    {
        Directory.Delete(imagesRleFolder, true);
        Directory.CreateDirectory(imagesRleFolder);
        
        Directory.Delete(imagesRleDecompressedFolder, true);
        Directory.CreateDirectory(imagesRleDecompressedFolder);

        foreach (var filename in Directory.GetFiles(imagesFolder))
        {
            var bytes = File.ReadAllBytes(filename);
            var rleBytes = RleEncode2(bytes);

            var destinationFile = Path.Combine(imagesRleFolder, Path.GetFileName(filename));
            File.WriteAllBytes(destinationFile, rleBytes);
        }
        
        foreach (var filename in Directory.GetFiles(imagesRleFolder))
        {
            var bytes = File.ReadAllBytes(filename);
            var rleBytes = RleDecode2(bytes);

            var destinationFile = Path.Combine(imagesRleDecompressedFolder, Path.GetFileName(filename));
            File.WriteAllBytes(destinationFile, rleBytes);
        }
    }
    
    //TODO: RLE in a block size (e.g. 3 as pixels are 3 bytes
    //TODO: Use double character a RLE trigger
}