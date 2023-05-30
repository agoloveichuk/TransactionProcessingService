namespace TransactionProcessingService.Services.Logging
{
    public class MetadataLogService : IMetadataLogService
    {
        private readonly ILoggingService loggingService;

        public MetadataLogService(ILoggingService loggingService)
        {
            this.loggingService = loggingService;
        }

        public void SaveMetadataLog(int parsedFiles, int parsedLines, int foundErrors, string[] invalidFilePaths, string metaLogFilePath)
        {
            string metaLog = $"parsed_files: {parsedFiles}\n" +
                             $"parsed_lines: {parsedLines}\n" +
                             $"found_errors: {foundErrors}\n" +
                             $"invalid_files: [{string.Join(", ", invalidFilePaths)}]";

            try
            {
                loggingService.LogMetaInformation(metaLog, metaLogFilePath);
                File.WriteAllText(metaLogFilePath, metaLog);
            }
            catch (Exception ex)
            {
                loggingService.LogValidationMessage($"Error occurred while saving metadata log: {ex.Message}", metaLogFilePath);
            }
        }
    }
}
