using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionProcessingService
{
    public class AppConfiguration
    {
        public string FolderA { get; set; }
        public string FolderB { get; set; }

        public static AppConfiguration LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The configuration file '{filePath}' does not exist.");
            }

            try
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<AppConfiguration>(json);
            }
            catch (JsonException ex)
            {
                throw new FormatException($"Error deserializing the configuration file '{filePath}'.", ex);
            }
        }
    }
}
