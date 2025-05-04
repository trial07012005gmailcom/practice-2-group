using PatientManager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PatientManager.Services
{
    public class PatientService
    {
        private readonly string filePath = "patients.txt";

        //CREATE LOGIC
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

        //GET ALL LOGIC 
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

        //UPDATE LOGIC 
        public bool UpdatePatient(string ci, string newName, string newLastName)
        {
            if (!File.Exists(filePath)) { return false; }

            var lines = File.ReadAllLines(filePath);    
            bool found = false;

            for (int i = 0; i < lines.Length; i++)
            {
                var data = lines[i].Split(",");

                if (data.Length == 4 && data[2].Trim() == ci)
                {
                    lines[i] = $"{newName},{newLastName},{data[2].Trim()},{data[3].Trim()}";
                    found = true;
                    break;
                } 
            }

            if (found)
            {
                File.WriteAllLines(filePath, lines);
            }

            return found;
        }

        //DELETE LOGIC 

        public bool DeletePatient(string ci)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }

            var lines = File.ReadAllLines(filePath);
            var updatedLines = new List<string>();
            bool found = false;

            foreach (var line in lines)
            {
                var data = line.Split(",");

                if (data.Length == 4 && data[2].Trim() == ci)
                {
                    found = true; //flag is true
                    continue; //omitimos la linea que queremos eliminar 
                }

                updatedLines.Add(line);
            }

            if (!found)
            {
                return false; //por si el ci no existe 
            }

            File.WriteAllLines(filePath, updatedLines);
            return true;
        }

        // GET BY CI LOGIC 

        public PatientWithBlood GetPatientByCi(string ci)
        {
            if (!File.Exists(filePath)) { return null; }

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var data = line.Split(",");

                if (data.Length == 4 && data[2].Trim() == ci)
                {
                    return new PatientWithBlood
                    {
                        Name = data[0].Trim(),
                        LastName = data[1].Trim(),
                        CI = data[2].Trim(),
                        BloodGroup = data[3].Trim(),
                    };
                }
            }

            return null; 
        }
    }
}
