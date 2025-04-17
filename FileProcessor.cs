using static System.Console;

namespace PSJR_FilePractice;

internal class FileProcessor
{
    private const string psDataDir = @"C:\Users\ddu41894\Desktop\psdata\";
    private const string psDataDirIn = @"C:\Users\ddu41894\Desktop\psdata\in\";
    private const string BackupDirectoryName = "backup";
    private const string InProgressDirectoryName = "procesing";
    private const string CompletedDirectoryName = "completed";
    string fileName;
    public string InputFilePath { get; }
    public FileProcessor(string filePath)
    {
        InputFilePath = filePath;
    }
    public void Process()
    {
        WriteLine($"processing {InputFilePath}");
        if (!File.Exists(InputFilePath))
        {
            WriteLine($"File does not exist in {InputFilePath}");
            fileName = Path.GetFileName(InputFilePath);
            return;
        }
        string? rootDirectoryPath = new DirectoryInfo(InputFilePath).Root.FullName;
        string backupDirectoryPath = Path.Combine(rootDirectoryPath, @"Users\ddu41894\Desktop\psdata\", BackupDirectoryName);
        if (rootDirectoryPath is null)
        { throw new Exception("Root directory path is null"); }
        if (!Directory.Exists(backupDirectoryPath))
        {
            WriteLine($"Creating backup directory: {backupDirectoryPath}");
            Directory.CreateDirectory(backupDirectoryPath);
        }

        WriteLine($"Root directory path: {rootDirectoryPath}");
        //create backup file
        string inputFileName = Path.GetFileName(InputFilePath);
        string backupFilePath = Path.Combine(backupDirectoryPath, inputFileName);
        WriteLine($"Creating backup file: {backupFilePath}");
        File.Copy(InputFilePath, backupFilePath, true);
        if (!Directory.Exists(Path.Combine(psDataDir, InProgressDirectoryName)))
        {
            WriteLine($"Creating in progress directory: {Path.Combine(psDataDir, InProgressDirectoryName)}");
            Directory.CreateDirectory(Path.Combine(psDataDir, InProgressDirectoryName));
        }
       MoveToInProgressDirectory(psDataDir);
        //determine filetype
        string extension = Path.GetExtension(InputFilePath);
        switch (extension)
        {
            case ".txt":
                ProcessTextFile(psDataDirIn);// in the video he uses inProgressFilePath, why do we need to use that?
                break;
            default:
                WriteLine($"File type {extension} not supported");
                break;
        }
        //Move to Completed Directory
       
        string completedDirectoryPath = Path.Combine(psDataDir, CompletedDirectoryName);
        Directory.CreateDirectory(completedDirectoryPath);
        //File.Move(InputFilePath, completedDirectoryPath);
        string fileNameCompletedExtension = Path.ChangeExtension(inputFileName, ".completed");
        string completedfileName = $"{Guid.NewGuid()}_{fileNameCompletedExtension}";
        string completedFilePath = Path.Combine(completedDirectoryPath, completedfileName);
        File.Move(InputFilePath, completedFilePath);

        //delete IP directory
        string? inProgressDirectoryPath = Path.Combine(psDataDir,InProgressDirectoryName);
        Directory.Delete(inProgressDirectoryPath!, true);//when this line of code executes it will delete the directory even if there are files inside
    }
    //move to IP dir
    private void MoveToInProgressDirectory(string psDataDir)
    {
        string fileNameMove = Path.Combine(psDataDir, InProgressDirectoryName, Path.GetFileName(InputFilePath));
        File.Move(InputFilePath, fileNameMove);
        File.Move(fileNameMove, InputFilePath); 
    }
    private void ProcessTextFile(string inProgressFilePath)
    {
        WriteLine($"Processing text file: {inProgressFilePath}");
        //read in and process the file
    }
}
