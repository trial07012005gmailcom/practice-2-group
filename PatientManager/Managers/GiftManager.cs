using Services.ExternalServices;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManager.Managers
{
    public class GiftManager
    {
        public GiftManager() { }

        public List<Electronic> GetGiftList()
        {
            ElectronicStoreService ess = new ElectronicStoreService(); 
            return ess.GetAllElectronicItems().Result;
        }
    }
}
