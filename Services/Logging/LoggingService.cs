namespace TransactionProcessingService.Services.Logging
{
    public class LoggingService : ILoggingService
    {
        public void LogValidationMessage(string message, string filePath)
        {
            string logFilePath = GetLogFilePath(filePath);

            try
            {
                using (StreamWriter writer = File.AppendText(logFilePath))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while logging validation message: {ex.Message}");
            }
        }

        public void LogMetaInformation(string metaInformation, string filePath)
        {
            string logFilePath = GetLogFilePath(filePath);

            try
            {
                using (StreamWriter writer = File.AppendText(logFilePath))
                {
                    writer.WriteLine(metaInformation);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while logging meta information: {ex.Message}");
            }
        }

        private string GetLogFilePath(string filePath)
        {
            string logDirectory = Path.GetDirectoryName(filePath);
            string logFileName = "meta.log";
            return Path.Combine(logDirectory, logFileName);
        }
    }
}
