using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Services.Models;

namespace PatientManager.Managers
{
    public class GiftManager
    {
        private readonly HttpClient _http;
        private readonly string _url;

        public GiftManager(IConfiguration config)
        {
            _http = new HttpClient();
            _url = config["GiftApi:Url"];
        }

        public async Task<List<Electronic>> GetGiftsAsync()
        {
            var response = await _http.GetAsync(_url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to get gifts from external API");

            var json = await response.Content.ReadAsStringAsync();

            var gifts = JsonSerializer.Deserialize<List<Electronic>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return gifts ?? new List<Electronic>();
        }
    }
}
