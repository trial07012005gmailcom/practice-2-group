using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Services.Models;
using Services.ExternalServices;
using PatientManager.Services;

namespace PatientManager.Managers
{
    public class GiftManager
    {
        private readonly ElectronicStoreService _store;

        public GiftManager(IConfiguration config)
        {
            _store = new ElectronicStoreService(config);
        }

        public async Task<List<Electronic>> GetGiftsAsync()
        {
            return await _store.GetAllElectronicItems();
        }

        public async Task<Electronic?> AssignGiftToPatient(string ci, PatientService patientService)
        {
            var patient = patientService.GetPatientByCi(ci);
            if (patient == null)
                return null;

            var gifts = await GetGiftsAsync();
            if (gifts.Count == 0)
                return null;

            var rnd = new Random();
            return gifts[rnd.Next(gifts.Count)];
        }

    }
}
