using System;
using Services.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Services.ExternalServices
{
    public class ElectronicStoreService
    {
        public ElectronicStoreService() { }

        public async Task<List<Electronic>>  GetAllElectronicItems()
        { 
            string url = "https://api.restful-api.dev/objects";

            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            List<Electronic> electronicItems = JsonConvert.DeserializeObject<List<Electronic>>(responseBody);
            return electronicItems; 
        }
    }
}
