using System.Linq;
using TransactionProcessingService.Models;

namespace TransactionProcessingService.Services.Transforming
{
    public class DataTransformer : IDataTransformer
    {
        public bool HasErrors { get; private set; }
        public List<string> Errors { get; private set; }

        public OutputModel TransformData(List<InputModel> paymentTransactions)
        {
            HasErrors = false;
            Errors = new List<string>();

            var outputModel = new OutputModel
            {
                Cities = new List<City>(),
                Total = 0
            };

            var cityGroups = paymentTransactions
                .GroupBy(t => t.Address.Split(',')[0].Trim()) // Group by the first part of the address (city name)
                .Select(g => new
                {
                    CityName = g.Key,
                    Transactions = g.ToList()
                });

            foreach (var cityGroup in cityGroups)
            {
                var city = new City
                {
                    Name = cityGroup.CityName,
                    Services = new List<Service>(),
                    Total = 0
                };

                var serviceGroups = cityGroup.Transactions
                    .GroupBy(t => t.Service)
                    .Select(g => new
                    {
                        ServiceName = g.Key,
                        Transactions = g.ToList()
                    });

                foreach (var serviceGroup in serviceGroups)
                {
                    var service = new Service
                    {
                        Name = serviceGroup.ServiceName,
                        Payers = serviceGroup.Transactions
                            .Select(t => new Payer
                            {
                                Name = $"{t.FirstName} {t.LastName}",
                                Payment = t.Payment,
                                Date = t.Date,
                                AccountNumber = t.AccountNumber
                            })
                            .ToList(),
                        Total = serviceGroup.Transactions.Sum(t => t.Payment)
                    };

                    city.Services.Add(service);
                    city.Total += service.Total;
                }

                outputModel.Cities.Add(city);
                outputModel.Total += city.Total;
            }

            return outputModel;
        }
    }
}
