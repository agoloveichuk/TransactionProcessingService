using System.Globalization;

namespace TransactionProcessingService.Services.Validation
{
    public class TypeConverter : ITypeConverter
    {
        public bool TryConvertToDecimal(string value, out decimal result)
        {
            return decimal.TryParse(value, out result);
        }

        public bool TryConvertToDateTime(string value, out DateTime result)
        {
            return DateTime.TryParseExact(value, "yyyy-dd-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        public bool TryConvertToLong(string value, out long result)
        {
            return long.TryParse(value, out result);
        }
    }
}
