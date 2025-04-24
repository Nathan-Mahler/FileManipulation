

namespace PSJR_FilePractice
{
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
}
