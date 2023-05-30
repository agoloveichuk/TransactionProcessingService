using Microsoft.Extensions.DependencyInjection;
using TransactionProcessingService;
using TransactionProcessingService.Services.Logging;
using TransactionProcessingService.Services.Processing;
using TransactionProcessingService.Services.Processing.Interfaces;
using TransactionProcessingService.Services.Transforming;
using TransactionProcessingService.Services.Validation;
using TransactionProcessingService.Services.Writing;
using TransactionProcessingService.Utils;

class Program
{
    private static CancellationTokenSource _cancellationTokenSource;

    static async Task Main()
    {
        Console.WriteLine("Hello, World!");

        try
        {
            await InitializeAndRunAsync();
        }
        catch (Exception ex)
        {
            // Handle and log any exceptions
            Console.WriteLine("An error occurred: " + ex.Message);
            // Log the exception using the logging service
            // loggingService.LogException(ex);
        }
    }

    static async Task InitializeAndRunAsync()
    {
        // Create the service collection
        var services = new ServiceCollection();

        // Register dependencies
        services.AddTransient<IDataTransformer, DataTransformer>();
        services.AddTransient<IFileWriter, FileWriter>();
        services.AddTransient<ILoggingService, LoggingService>();
        services.AddTransient<IMetadataLogService, MetadataLogService>();
        services.AddTransient<ITypeConverter, TypeConverter>();
        services.AddTransient<IDataValidator, DataValidator>();
        services.AddTransient<IFileProcessorFactory, FileProcessorFactory>();

        // Build the service provider
        var serviceProvider = services.BuildServiceProvider();

        // Resolve the dependencies
        var fileProcessorFactory = serviceProvider.GetRequiredService<IFileProcessorFactory>();

        AppConfiguration configuration = AppConfiguration.LoadFromFile("D:\\Projects\\TransactionProcessingService\\appsettings.json");
        string folderA = configuration.FolderA;
        string folderB = configuration.FolderB;

        // Start the file watcher
        var fileWatcher = new FileWatcher(folderA, fileProcessorFactory);

        // Set up cancellation
        _cancellationTokenSource = new CancellationTokenSource();

        // Start the watcher
        var watcherTask = fileWatcher.StartWatchingAsync(_cancellationTokenSource.Token);

        // Wait for cancellation
        await WaitForCancellationAsync();

        // Stop the watcher
        _cancellationTokenSource.Cancel();
        await watcherTask;
    }

    static async Task WaitForCancellationAsync()
    {
        Console.WriteLine("Press Q to quit.");

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Q)
            {
                _cancellationTokenSource.Cancel();
                break;
            }
        }
    }
}
