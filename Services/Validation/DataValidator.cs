using TransactionProcessingService.Services.Logging;

namespace TransactionProcessingService.Services.Validation
{
    public class DataValidator : IDataValidator
    {
        private readonly ILoggingService loggingService;
        private readonly ITypeConverter typeConverter;
        private readonly IMetadataLogService metadataLogService;

        public int InvalidLinesCount { get; private set; }
        public int InvalidFilesCount { get; private set; }
        public List<string> InvalidFiles { get; private set; }

        public DataValidator(ILoggingService loggingService, ITypeConverter typeConverter, IMetadataLogService metadataLogService)
        {
            this.loggingService = loggingService;
            this.typeConverter = typeConverter;
            this.metadataLogService = metadataLogService;
            InvalidLinesCount = 0;
            InvalidFilesCount = 0;
            InvalidFiles = new List<string>();
        }

        public bool ValidateLine(string line, int lineNumber, string filePath)
        {
            string[] fields = line.Split(',');

            // Check if the line has the correct number of fields
            if (fields.Length != 9)
            {
                InvalidLinesCount++;
                loggingService.LogValidationMessage($"Invalid line length (line number: {lineNumber + 1}): {line}", filePath);
                return false;
            }
            else
            {
                // Validate the line fields
                if (!ValidateLineFields(fields, lineNumber, line, filePath))
                {
                    InvalidLinesCount++;
                    return false;
                }
            }

            if (InvalidLinesCount > 0)
                LogValidationErrors(filePath);

            return true;
        }

        private bool ValidateLineFields(string[] fields, int lineNumber, string line, string filePath)
        {
            // Validate first_name
            if (string.IsNullOrEmpty(fields[0].Trim()))
            {
                loggingService.LogValidationMessage($"Invalid first name (line number: {lineNumber + 1}): {line}", filePath);
                return false;
            }

            // Validate last_name
            if (string.IsNullOrEmpty(fields[1].Trim()))
            {
                loggingService.LogValidationMessage($"Invalid last name (line number: {lineNumber + 1}): {line}", filePath);
                return false;
            }

            // Validate address
            string address = string.Join(",", fields.Skip(2).Take(4)).Trim();
            if (string.IsNullOrEmpty(address))
            {
                loggingService.LogValidationMessage($"Invalid address (line number: {lineNumber + 1}): {line}", filePath);
                return false;
            }

            // Validate payment
            if (!typeConverter.TryConvertToDecimal(fields[5].Trim(), out _))
            {
                loggingService.LogValidationMessage($"Invalid payment value (line number: {lineNumber + 1}): {line}", filePath);
                return false;
            }

            // Validate date
            if (!typeConverter.TryConvertToDateTime(fields[6].Trim(), out _))
            {
                loggingService.LogValidationMessage($"Invalid date value (line number: {lineNumber + 1}): {line}", filePath);
                return false;
            }

            // Validate account_number
            if (!typeConverter.TryConvertToLong(fields[7].Trim(), out _))
            {
                loggingService.LogValidationMessage($"Invalid account number value (line number: {lineNumber + 1}): {line}", filePath);
                return false;
            }

            // Validate service
            if (string.IsNullOrEmpty(fields[8].Trim()))
            {
                loggingService.LogValidationMessage($"Invalid service (line number: {lineNumber + 1}): {line}", filePath);
                return false;
            }

            return true;
        }

        public void LogValidationErrors(string filePath)
        {
            InvalidFilesCount++;
            InvalidFiles.Add(filePath);
            loggingService.LogValidationMessage($"Invalid file: {filePath}", "meta.log");

            // Save the metadata log
            int parsedFilesCount = InvalidFilesCount;
            int parsedLinesCount = InvalidLinesCount;
            int foundErrorsCount = InvalidFilesCount;
            string[] invalidFilePaths = InvalidFiles.ToArray();
            string metaLogFiles = "meta.log";

            metadataLogService.SaveMetadataLog(parsedFilesCount, parsedLinesCount, foundErrorsCount, invalidFilePaths, metaLogFiles);
        }
    }
}
