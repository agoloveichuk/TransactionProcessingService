namespace TransactionProcessingService.Models
{
    public class City
    {
        public required string Name { get; set; }
        public required List<Service> Services { get; set; }
        public required decimal Total { get; set; }
    }
}
