namespace TransactionProcessingService.Services.Processing.Interfaces
{
    public interface IFileProcessorFactory
    {
        IFileProcessor CreateFileProcessor(string filePath);
    }
}
