using TransactionProcessingService.Models;

namespace TransactionProcessingService.Services.Writing
{
    public interface IFileWriter
    {
        void WriteToFile(OutputModel output, string filePath);
    }
}
