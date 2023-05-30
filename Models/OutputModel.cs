namespace TransactionProcessingService.Models
{
    public class OutputModel
    {
        public required List<City> Cities { get; set; }
        public required decimal Total { get; set; }
    }
}
