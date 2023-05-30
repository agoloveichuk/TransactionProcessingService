namespace TransactionProcessingService.Services.Validation
{
    public interface ITypeConverter
    {
        bool TryConvertToDecimal(string value, out decimal result);
        bool TryConvertToDateTime(string value, out DateTime result);
        bool TryConvertToLong(string value, out long result);
    }
}
