using static CompressLib.HexParser;
using static CompressLib.RunLengthEncoding;

namespace CompressLib;

public class RunLengthEncodingTests
{
    [Fact]
    public void RleText()
    {
        var textToRle = "01 01 01";
        
        var expectedRleTest = "01 03";
        
        Assert.Equal(expectedRleTest, RleEncode(textToRle));
    }
    
    [Fact]
    public void RleTextWithMoreThat256Repeat()
    {
        string textToRle =  String.Join("", Enumerable.Repeat("01 ", 510)).Trim();
        
        var expectedRleTest = "01 FF 01 FF";
        
        Assert.Equal(expectedRleTest, RleEncode(textToRle));
    }
    
    [Fact]
    public void DecodeRleText()
    {
        var textToRleDecode = "01 03";
        
        var expectedDecodedText = "01 01 01";
        
        Assert.Equal(expectedDecodedText, RleDecode(textToRleDecode));
    }

    [Fact]
    public void CompressSif()
    {
        var imageBytes = File.ReadAllBytes(@"/Users/adampridmore/work/Dev/york-code-dojo/OutOfSpace/Images/_DSC2240_DxO.sif");
        var convertedBytes = RleEncode(imageBytes);

        var destinationFileRle = @"/Users/adampridmore/work/Dev/york-code-dojo/OutOfSpace/ImagesRle/_DSC2240_DxO.sif";
        File.WriteAllBytes(destinationFileRle, convertedBytes);

        var decodeBytes = RleDecode(convertedBytes);

        var destinationFileDecoded = @"/Users/adampridmore/work/Dev/york-code-dojo/OutOfSpace/ImagesRleDecompressed/_DSC2240_DxO.sif";

        File.WriteAllBytes(destinationFileDecoded, decodeBytes);
    }

    [Fact]
    public void RleAllFiles()
    {
        var imagesFolder = "/Users/adampridmore/work/Dev/york-code-dojo/OutOfSpace/Images";
        var destinationFolder = "/Users/adampridmore/work/Dev/york-code-dojo/OutOfSpace/ImagesRle";
        
        Directory.Delete(destinationFolder, true);
        Directory.CreateDirectory(destinationFolder);

        foreach (var filename in Directory.GetFiles(imagesFolder))
        {
            var bytes = File.ReadAllBytes(filename);
            var rleBytes = RleEncode(bytes);

            var destinationFile = Path.Combine(destinationFolder, Path.GetFileName(filename));
            File.WriteAllBytes(destinationFile, rleBytes);
        }
    }
    
    //TODO: RLE in a block size (e.g. 3 as pixels are 3 bytes
    //TODO: Use double character a RLE trigger
}