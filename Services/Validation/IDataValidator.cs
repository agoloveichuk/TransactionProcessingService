namespace TransactionProcessingService.Services.Validation
{
    public interface IDataValidator
    {
        bool ValidateLine(string line, int lineNumber, string filePath);
        int InvalidLinesCount { get; }
        int InvalidFilesCount { get; }
        List<string> InvalidFiles { get; }
        void LogValidationErrors(string filePath);
    }
}
