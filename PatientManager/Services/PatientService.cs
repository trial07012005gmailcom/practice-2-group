using PatientManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManager.Services
{
    public class PatientService
    {
        private readonly string filePath = "patients.txt";

        public PatientWithBlood CreatePatient(Patient patient)
        {
            string[] bloodGroups = { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
            string randomBlood = bloodGroups[new Random().Next(bloodGroups.Length)];

            var fullPatient = new PatientWithBlood
            {
                Name = patient.Name,
                LastName = patient.LastName,
                CI = patient.CI,
                BloodGroup = randomBlood
            };

            string line = $"{fullPatient.Name},{fullPatient.LastName},{fullPatient.CI},{fullPatient.BloodGroup}";

            System.IO.File.AppendAllLines(filePath, new[] { line });

            return fullPatient;
        }

        public List<PatientWithBlood> GetAllPatients()
        {
            var patients = new List<PatientWithBlood>();

            if (!System.IO.File.Exists(filePath))
                return patients;

            var lines = System.IO.File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var data = line.Split(',');

                if (data.Length == 4)
                {
                    patients.Add(new PatientWithBlood
                    {
                        Name = data[0].Trim(),
                        LastName = data[1].Trim(),
                        CI = data[2].Trim(),
                        BloodGroup = data[3].Trim()
                    });
                }
            }

            return patients;
        }
    }
}
