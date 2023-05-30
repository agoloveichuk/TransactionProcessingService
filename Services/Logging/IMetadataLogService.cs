namespace TransactionProcessingService.Services.Logging
{
    public interface IMetadataLogService
    {
        void SaveMetadataLog(int parsedFiles, int parsedLines, int foundErrors, string[] invalidFilePaths, string metaLogFilePath);
    }
}
