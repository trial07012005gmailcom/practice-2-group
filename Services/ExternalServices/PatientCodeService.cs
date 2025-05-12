using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Services.Models;

namespace Services.ExternalServices
{
    public class PatientCodeService
    {
        private readonly HttpClient _httpClient;

        public PatientCodeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetPatientCodeAsync(string name, string lastName, string ci)
        {
            //Endpoint Azure
            string baseUrl = "https://practice-3-hqeuemhvd2cwevhp.centralus-01.azurewebsites.net";
            string endpoint = $"/api/patientcode?name={name}&lastName={lastName}&ci={ci}";

            var response = await _httpClient.GetAsync(baseUrl + endpoint);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PatientCodeResponse>(json);
            return result.PatientCode;
        }
    }
}
