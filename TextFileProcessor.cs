

namespace PSJR_FilePractice;


internal class TextFileProcessor
{
    public string InputFilePath { get; }
    public string OutputFilePath { get; }
    public TextFileProcessor(string inputFilePath, string outputFilePath)
    {
        InputFilePath = inputFilePath;
        OutputFilePath = outputFilePath;
    }

    public void Process()
    {
        //process as string
        //string orginalText = File.ReadAllText(InputFilePath + "\\data1.txt");
        //string processedText = orginalText.ToUpper();
        //Console.WriteLine(  processedText);
        //process as array
        // Read the file
        string[] lines = File.ReadAllLines(InputFilePath);
        
        // Process the file
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].ToUpper();
        }
        // Write the file
        File.WriteAllLines(OutputFilePath, lines);
    }
}
