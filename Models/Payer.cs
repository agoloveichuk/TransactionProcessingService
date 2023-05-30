namespace TransactionProcessingService.Models
{
    public class Payer
    {
        public required string Name { get; set; }
        public required decimal Payment { get; set; }
        public required DateTime Date { get; set; }
        public required long AccountNumber { get; set; }
    }
}
