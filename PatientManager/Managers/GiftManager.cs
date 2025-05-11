using Services.Models;
using Services.ExternalServices;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientManager.Managers
{
    public class GiftManager
    {
        private readonly IConfiguration _config;
        private readonly ElectronicStoreService _store;

        public GiftManager(IConfiguration config, ElectronicStoreService store)
        {
            _config = config;
            _store = store;
        }

        public async Task<List<Electronic>> GetGiftsAsync()
        {
            return await _store.GetAllElectronicItems(_config);
        }

        public async Task<Electronic?> AssignGiftToPatient(string ci, Services.PatientService patientService)
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
