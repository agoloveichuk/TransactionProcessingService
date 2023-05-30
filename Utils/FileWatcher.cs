using System;
using System.IO;
using System.Threading.Tasks;
using TransactionProcessingService.Services.Processing.Interfaces;

namespace TransactionProcessingService.Utils
{
    public class FileWatcher
    {
        private readonly FileSystemWatcher _watcher;
        private readonly IFileProcessorFactory _fileProcessorFactory;

        public FileWatcher(string folderPath, IFileProcessorFactory fileProcessorFactory)
        {
            _watcher = new FileSystemWatcher(folderPath);
            _fileProcessorFactory = fileProcessorFactory;
        }

        public async Task StartWatchingAsync(CancellationToken cancellationToken)
        {
            _watcher.Filter = "*.*";
            _watcher.Created += OnFileCreated;
            _watcher.EnableRaisingEvents = true;

            // Wait for cancellation
            await Task.Delay(-1, cancellationToken);
        }

        public void StopWatching()
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Created -= OnFileCreated;
            _watcher.Dispose();
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            string filePath = e.FullPath;
            string extension = Path.GetExtension(filePath);

            // Filter out files that are not in TXT or CSV format
            if (extension.Equals(".txt", StringComparison.OrdinalIgnoreCase) ||
                extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                // Process the file immediately upon detection
                ProcessFile(filePath);
            }
        }

        private void ProcessFile(string filePath)
        {
            Console.WriteLine($"Processing file: {filePath}");
            IFileProcessor fileProcessor = _fileProcessorFactory.CreateFileProcessor(filePath);
            fileProcessor.ProcessFile(filePath);
        }
    }
}
