using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Services.Models;
using Services.ExternalServices;

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
    }
}
