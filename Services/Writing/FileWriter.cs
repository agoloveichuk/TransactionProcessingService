using Newtonsoft.Json;
using System;
using System.IO;
using TransactionProcessingService.Models;

namespace TransactionProcessingService.Services.Writing
{
    public class FileWriter : IFileWriter
    {
        public void WriteToFile(OutputModel output, string resultFolderPath)
        {
            string currentDate = DateTime.Now.ToString("MM-dd-yyyy");
            string resultFilePath = Path.Combine(resultFolderPath, currentDate, "result.txt");

            // Check if the directory for the current date exists, create it if it doesn't
            Directory.CreateDirectory(Path.GetDirectoryName(resultFilePath));

            string existingJson = "";
            if (File.Exists(resultFilePath))
            {
                existingJson = File.ReadAllText(resultFilePath);
            }

            string newJson = JsonConvert.SerializeObject(output, Formatting.Indented);
            string combinedJson = MergeJson(existingJson, newJson);

            File.WriteAllText(resultFilePath, combinedJson);
        }

        private string MergeJson(string existingJson, string newJson)
        {
            if (string.IsNullOrWhiteSpace(existingJson))
            {
                return newJson;
            }

            var existingOutput = JsonConvert.DeserializeObject<OutputModel>(existingJson);
            var newOutput = JsonConvert.DeserializeObject<OutputModel>(newJson);

            foreach (var newCity in newOutput.Cities)
            {
                var existingCity = existingOutput.Cities.FirstOrDefault(c => c.Name == newCity.Name);
                if (existingCity != null)
                {
                    foreach (var newService in newCity.Services)
                    {
                        var existingService = existingCity.Services.FirstOrDefault(s => s.Name == newService.Name);
                        if (existingService != null)
                        {
                            existingService.Payers.AddRange(newService.Payers);
                            existingService.Total += newService.Total;
                        }
                        else
                        {
                            existingCity.Services.Add(newService);
                        }
                    }
                }
                else
                {
                    existingOutput.Cities.Add(newCity);
                }
            }

            existingOutput.Total += newOutput.Total;

            return JsonConvert.SerializeObject(existingOutput, Formatting.Indented);
        }
    }
}
