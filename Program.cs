using PSJR_FilePractice;
using static System.Console;

namespace PSJR_FilePractice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Parsing command line options");
            var command = args[0];

            if (command == "--file")
            {
                string filePath = args[1];
                //check if path is absolute or relative
                if (!Path.IsPathFullyQualified(filePath))
                {
                    WriteLine($"File path is not absolute: {filePath}");
                    ReadLine();
                    return;
                }
                WriteLine($"Processing file: {filePath}");
                ProcessSingleFile(filePath);
            }
            else if (command == "--dir")
            {
                string directoryPath = args[1];
                string fileType = args[2];
                WriteLine($"Processing directory: {directoryPath} + for {fileType} files");
                ProcessDirectory(directoryPath, fileType);
            }
            WriteLine("Done");
            ReadLine();
        }
        private static void ProcessSingleFile(string filePath)
        {
            FileProcessor fileProcessor = new FileProcessor(filePath);
            fileProcessor.Process();
        }
        private static void ProcessDirectory(string directoryPath, string fileType)
        {
            switch (fileType) 
            {
                case "TEXT":
                    string[] txtFiles = Directory.GetFiles(directoryPath, "*.txt");//overload of GetFiles method where we are searching a directory for a certain type of file
                    foreach (string txtFile in txtFiles) 
                    { 
                        var fileProcessor = new FileProcessor(txtFile);
                        fileProcessor.Process();
                    }
                    break;
                default:
                    WriteLine($"File type {fileType} not supported");
                    break;
            }

        }

       
    }
}
