namespace TransactionProcessingService.Models
{
    public class InputModel
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Address { get; set; }
        public required decimal Payment { get; set; }
        public required DateTime Date { get; set; }
        public required long AccountNumber { get; set; }
        public required string Service { get; set; }
    }
}