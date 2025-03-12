// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using System.Text;

Console.WriteLine("Hello, World!");

var imagesFolder = "/Users/adampridmore/work/Dev/york-code-dojo/OutOfSpace/Images";
var destinationFolder = "/Users/adampridmore/work/Dev/york-code-dojo/OutOfSpace/ImagesCompressed";
var fileNames = Directory.GetFiles(imagesFolder);


// Directory.GetFiles(destinationFolder);
Directory.Delete(destinationFolder, true);
Directory.CreateDirectory(destinationFolder);


using var compressStreamWritter = File.CreateText(Path.Combine(destinationFolder, "compress.txt"));

string ComputeHash(byte[] image)
{
    var data = SHA256.HashData(image);
    var sBuilder = new StringBuilder();
    foreach (var t in data)
        sBuilder.Append(t.ToString("x2"));
    return sBuilder.ToString();
}

var fileHashes = new Dictionary<string, List<string>>();

foreach (var filename in (fileNames))
{
    var fileHash = ComputeHash(File.ReadAllBytes(filename));
    if (fileHashes.TryGetValue(fileHash, out var matchingFileNames))
    {
        matchingFileNames.Add(filename);
    }
    else
    {
        fileHashes.Add(fileHash, new List<string>(new []{filename}));
    }
}


foreach (var keyValue in fileHashes)
{
    var fileNamesToCopy = keyValue.Value;

    if (fileNamesToCopy.Count == 1)
    {
        var filename = fileNamesToCopy.Single();
        var destinationFilename = Path.Combine(destinationFolder, Path.GetFileName(filename));
        
        File.Copy(filename, destinationFilename);        
    }
    else
    {
        var filename = fileNamesToCopy.First();
        var destinationFilename = Path.Combine(destinationFolder, Path.GetFileName(filename));
        
        File.Copy(filename, destinationFilename);
        
        // TODO: Make note somewhere that there's a copy
        foreach (var fileNameThatIsACopy in fileNamesToCopy.Skip(1))
        {
            var compressionLine = $"{Path.GetFileName(filename)} COPY {Path.GetFileName(fileNameThatIsACopy)} ";
            compressStreamWritter.WriteLine(compressionLine);    
        }
    }
    
    // Console.WriteLine(filename);
    //
    // var destinationFilename = Path.Combine(destinationFolder, Path.GetFileName(filename));
    //
    // File.Copy(filename, destinationFilename);
}

foreach (var keyValue in fileHashes)
{
    Console.WriteLine($"hash: {keyValue.Key} filename {keyValue.Value.Count}");
    // foreach (var filename in keyValue.Value)
    // {
    //     Console.WriteLine($"hash: {keyValue.Key} filename {filename}");    
    // }
    // // Console.WriteLine($"hash: {keyValue.Key} filename {keyValue.Value.Count}");
}


