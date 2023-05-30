namespace TransactionProcessingService.Services.Logging
{
    public interface ILoggingService
    {
        void LogValidationMessage(string message, string filePath);
        void LogMetaInformation(string metaInformation, string filePath);
    }
}
