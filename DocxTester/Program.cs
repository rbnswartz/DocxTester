using USFMToolsSharp;
using USFMToolsSharp.Models.Markers;
using USFMToolsSharp.Renderers.Docx;
var input = "\\ c 1 \\p \\v 1 This is a test";
var path = "/home/reuben/Downloads/tmp/psa.usfm";
var folderPath = "../../../../../../repos/arb_nav/";
var parser = new USFMParser();
var renderer = new OOXMLDocxRenderer(new DocxConfig() {rightToLeft = true});
var output = renderer.Render(LoadFile(path));
foreach (var problematicMarkers in renderer.UnrenderableMarkers.Distinct())
{
    Console.WriteLine(problematicMarkers);
}
await using var file = File.OpenWrite($"output.docx");
output.Position = 0;
await output.CopyToAsync(file);
file.Flush();

USFMDocument LoadDirectory(string inputPath)
{
    ArgumentNullException.ThrowIfNull(inputPath, nameof(inputPath));
    if(!Directory.Exists(inputPath))
    {
        throw new DirectoryNotFoundException($"Directory {inputPath} does not exist");
    }
    
    var document = new USFMDocument();
    foreach (var filePath in Directory.EnumerateFiles(inputPath,"*.usfm"))
    {
        Console.WriteLine(Path.GetFileName(filePath));
        document.Insert(parser.ParseFromString(File.ReadAllText(filePath)));
    }

    return document;
}

USFMDocument LoadFile(string inputPath)
{
    ArgumentNullException.ThrowIfNull(path, nameof(inputPath));
    if (!File.Exists(inputPath))
    {
        throw new FileNotFoundException($"File {inputPath} does not exist");
    }
    return parser.ParseFromString(File.ReadAllText(inputPath));
}
