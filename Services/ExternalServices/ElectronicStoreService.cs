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
            _url = config["GiftApi:Url"];  // 🔥 Valor leído desde appsettings.json
        }

        public async Task<List<Electronic>> GetAllElectronicItems(IConfiguration config)
        {
            var response = await _http.GetAsync(_url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var electronicItems = JsonSerializer.Deserialize<List<Electronic>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return electronicItems ?? new List<Electronic>();
        }
    }
}
