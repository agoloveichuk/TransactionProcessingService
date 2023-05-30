using TransactionProcessingService.Services.Processing.Interfaces;
using TransactionProcessingService.Services.Transforming;
using TransactionProcessingService.Services.Validation;
using TransactionProcessingService.Services.Writing;

namespace TransactionProcessingService.Services.Processing
{
    public class FileProcessorFactory : IFileProcessorFactory
    {
        private readonly IDataTransformer dataTransformer;
        private readonly IDataValidator dataValidator;
        private readonly IFileWriter writer;

        public FileProcessorFactory(IDataTransformer dataTransformer, IDataValidator dataValidator, IFileWriter writer)
        {
            this.dataTransformer = dataTransformer;
            this.dataValidator = dataValidator;
            this.dataValidator = dataValidator;
            this.writer = writer;
        }

        public IFileProcessor CreateFileProcessor(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            if (extension.Equals(".txt", StringComparison.OrdinalIgnoreCase))
            {
                return new TxtFileProcessor(dataTransformer, dataValidator, writer);
            }
            else
            {
                return new CsvFileProcessor(dataTransformer, dataValidator, writer);
            }
        }
    }
}
