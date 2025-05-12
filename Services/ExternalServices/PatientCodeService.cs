using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Services.Models;
using Microsoft.Extensions.Configuration;

namespace Services.ExternalServices
{
    public class PatientCodeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string[] _bloodTypes = { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };

        public PatientCodeService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _baseUrl = config["PatientCodeService:BaseUrl"];
        }

        public async Task<string> GetPatientCodeAsync(string name, string lastName, string ci)
        {
            var random = new Random();
            var bloodType = _bloodTypes[random.Next(_bloodTypes.Length)];

            var body = new
            {
                Name = name,
                LastName = lastName,
                CI = ci,
                BloodType = bloodType
            };

            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 🔧 Ruta corregida con mayúsculas
            var url = $"{_baseUrl}/api/PatientCode/generate";

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PatientCodeResponse>(jsonResponse);

            return result.PatientCode;
        }


    }
}
