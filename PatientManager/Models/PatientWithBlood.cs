using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManager.Models
{
    public class PatientWithBlood : Patient
    {
        public string BloodGroup { get; set; }
    }
}
