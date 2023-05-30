namespace TransactionProcessingService.Models
{
    public class Service
    {
        public required string Name { get; set; }
        public required List<Payer> Payers { get; set; }
        public required decimal Total { get; set; }
    }
}
