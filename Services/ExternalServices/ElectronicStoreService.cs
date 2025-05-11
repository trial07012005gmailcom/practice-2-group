using Services.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Services.ExternalServices
{
    public class ElectronicStoreService
    {
        private readonly HttpClient _http;
        private readonly string _url;

        public ElectronicStoreService(IConfiguration config)
        {
            _http = new HttpClient();
            _url = config["GiftApi:Url"];
        }

        public async Task<List<Electronic>> GetAllElectronicItems()
        {
            var response = await _http.GetAsync(_url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var items = JsonSerializer.Deserialize<List<Electronic>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return items ?? new List<Electronic>();
        }
    }
}
