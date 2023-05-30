using System.Globalization;
using TransactionProcessingService.Models;
using TransactionProcessingService.Services.Processing.Interfaces;
using TransactionProcessingService.Services.Transforming;
using TransactionProcessingService.Services.Validation;
using TransactionProcessingService.Services.Writing;


namespace TransactionProcessingService.Services.Processing
{
    public class CsvFileProcessor : IFileProcessor
    {
        private readonly IDataTransformer dataTransformer;
        private readonly IDataValidator dataValidator;
        private readonly IFileWriter writer;
        private bool isShutdownRequested;

        public CsvFileProcessor(IDataTransformer dataTransformer, IDataValidator dataValidator, IFileWriter writer)
        {
            this.dataTransformer = dataTransformer;
            this.dataValidator = dataValidator;
            this.writer = writer;
            this.isShutdownRequested = false;
        }

        public void ProcessFile(string filePath)
        {
            Console.WriteLine($"Processing TXT file: {filePath}");
            var paymentTransactions = new List<InputModel>();

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                bool isFirstLine = false;

                for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
                {
                    if (isShutdownRequested)
                    {
                        Console.WriteLine("Shutdown requested. Aborting file processing.");
                        return;
                    }

                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }

                    string line = lines[lineNumber];
                    bool check = dataValidator.ValidateLine(line, lineNumber, filePath);

                    if (!check)
                        continue;

                    string[] fields = line.Split(',');

                    string firstName = fields[0].Trim('\"');
                    string lastName = fields[1].Trim();
                    string address = fields[2].Trim().Trim('“', '”');
                    decimal payment = decimal.Parse(fields[5].Trim());

                    if (!DateTime.TryParseExact(fields[6].Trim(), "yyyy-dd-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                    {
                        date = DateTime.MinValue;
                    }

                    long accountNumber = long.Parse(fields[7].Trim().Trim('“', '”'));
                    string service = fields[8].Trim().Trim('\"');

                    var paymentTransaction = new InputModel
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        Payment = payment,
                        Date = date,
                        AccountNumber = accountNumber,
                        Service = service
                    };

                    paymentTransactions.Add(paymentTransaction);
                }

                if (isShutdownRequested)
                {
                    Console.WriteLine("Shutdown requested. Aborting file processing.");
                    return;
                }

                OutputModel output = dataTransformer.TransformData(paymentTransactions);
                Console.WriteLine("Processing complete.");
                string resultFilePath = "D:\\Testing\\TransationsProject\\FolderA\\FolderB";
                writer.WriteToFile(output, resultFilePath);
                Console.WriteLine("Writing complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing file '{filePath}': {ex.Message}");
                // You can log the error using your logging service
            }
        }

        public void RequestShutdown()
        {
            isShutdownRequested = true;
        }
    }
}
