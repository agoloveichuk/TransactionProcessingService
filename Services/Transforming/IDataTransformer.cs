using TransactionProcessingService.Models;

namespace TransactionProcessingService.Services.Transforming
{
    public interface IDataTransformer
    {
        OutputModel TransformData(List<InputModel> paymentTransactions);
        bool HasErrors { get; }
        List<string> Errors { get; }
    }
}
