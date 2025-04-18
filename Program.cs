using PSJR_FilePractice;
using static System.Console;

namespace PSJR_FilePractice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Parsing command line options");
            var directoryToWatch = args[0];
            if (!Directory.Exists(directoryToWatch))
            {
                WriteLine($"{directoryToWatch} does not exist.");
                WriteLine("Press enter to quit");
                ReadLine();
                return;
            }
            WriteLine($"Watching directory: {directoryToWatch}");
            using FileSystemWatcher inputFileWatcher = new FileSystemWatcher(directoryToWatch);

            inputFileWatcher.IncludeSubdirectories = false;
            inputFileWatcher.InternalBufferSize = 32_768; // 32kB is the size of this property
            inputFileWatcher.Filter = "*.*"; //this is the default
            inputFileWatcher.NotifyFilter = NotifyFilters.FileName;

            inputFileWatcher.Created += FileCreated;
            inputFileWatcher.Changed += FileChanged;
            inputFileWatcher.Deleted += FileDeleted;
            inputFileWatcher.Renamed += FileRenamed;
            inputFileWatcher.Error += WatcherError;

            inputFileWatcher.EnableRaisingEvents = true;
            WriteLine("Press enter to quit");
            ReadLine();
        }

        private static void WatcherError(object sender, ErrorEventArgs e)
        {
            WriteLine($"Folder PS data in no longer being watched  with error: {e.GetException}");
        }

        private static void FileRenamed(object sender, RenamedEventArgs e)
        {
            WriteLine($"File renamed: {e.FullPath} - e type: {e.ChangeType}");
        }

        private static void FileDeleted(object sender, FileSystemEventArgs e)
        {
            WriteLine($"File deleted: {e.FullPath} - e type: {e.ChangeType}");
        }

        private static void FileChanged(object sender, FileSystemEventArgs e)
        {
            WriteLine($"File changed: {e.FullPath} - e type: {e.ChangeType}");
        }

        private static void FileCreated(object sender, FileSystemEventArgs e)
        {
            WriteLine($"File created: {e.FullPath} - e type: {e.ChangeType}");
        }

        private static void ProcessSingleFile(string filePath)
        {
            FileProcessor fileProcessor = new FileProcessor(filePath);
            fileProcessor.Process();
        }
    }
}
