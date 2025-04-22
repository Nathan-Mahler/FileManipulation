using PSJR_FilePractice;
using static System.Console;
using System.Collections.Concurrent;
using System.Runtime.Caching;

namespace PSJR_FilePractice
{
    internal class Program
    {
        public static ConcurrentDictionary<string, string> Files = new();
        public static MemoryCache MemFiles = MemoryCache.Default;
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
            using var timer = new Timer(ProcessFiles, null, 0, 1000);
            inputFileWatcher.IncludeSubdirectories = false;
            inputFileWatcher.InternalBufferSize = 32_768; // 32kB is the size of this property
            inputFileWatcher.Filter = "*.*"; //this is the default
            inputFileWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

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
            WriteLine($"Folder PS data in no longer being watched  with error: {e.GetException()}");
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
            Files.TryAdd(e.FullPath, e.FullPath);
            AddToCache(e.FullPath);
        }

        private static void FileCreated(object sender, FileSystemEventArgs e)
        {
            WriteLine($"File created: {e.Name} - e type: {e.ChangeType}");
            Files.TryAdd(e.FullPath,e.FullPath);
            AddToCache(e.FullPath);
        }

        private static void ProcessFiles(object stateInfo)
        {
            foreach (var fileName in Files.Keys)
            {
                if (Files.TryRemove(fileName, out _))// we used the TryRemove because if is already there then we can remove it since we just added it. If it fails to remove the object it will return false and so the loop will not be entered into so it will not try to process a file it has presumably already processed, it does not however prevent us from processing the same file at different times. To prevent this we could store the completed files in a dictionary  and   and only process files if they are not in that dictionary.         
                {
                    var fileProcessor = new FileProcessor(fileName);
                    fileProcessor.Process();
                }
                
            }
        }
        private static void AddToCache(string fullPath)
        {
            var item = new CacheItem(fullPath, fullPath);

            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1)
            };
        }
    }
}
