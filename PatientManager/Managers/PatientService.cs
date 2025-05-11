using PatientManager.Models;
using Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PatientManager.Services
{
    public class PatientService
    {
        private readonly string filePath = "patients.txt";

        public PatientWithBlood CreatePatient(Patient patient)
        {
            if (patient == null || string.IsNullOrWhiteSpace(patient.Name) || string.IsNullOrWhiteSpace(patient.LastName) || string.IsNullOrWhiteSpace(patient.CI))
            {
                throw new ArgumentNullException("Datos del paciente inválidos.");
            }

            var exists = GetAllPatients().FirstOrDefault(p => p.CI == patient.CI);
            if (exists != null)
            {
                throw new ArgumentException($"Ya existe un paciente con CI {patient.CI}");
            }

            var fullPatient = new PatientWithBlood
            {
                Name = patient.Name,
                LastName = patient.LastName,
                CI = patient.CI,
                BloodGroup = GetRandomBloodGroup()
            };

            File.AppendAllLines(filePath, new[] { ToLine(fullPatient) });
            return fullPatient;
        }

        public List<PatientWithBlood> GetAllPatients()
        {
            if (!File.Exists(filePath))
                return new List<PatientWithBlood>();

            return File.ReadAllLines(filePath)
                       .Where(line => !string.IsNullOrWhiteSpace(line))
                       .Select(ToPatient)
                       .ToList();
        }

        public PatientWithBlood GetPatientByCi(string ci)
        {
            if (string.IsNullOrWhiteSpace(ci) || !File.Exists(filePath))
                return null;

            return GetAllPatients().FirstOrDefault(p => p.CI == ci);
        }

        public bool UpdatePatient(string ci, string newName, string newLastName)
        {
            if (string.IsNullOrWhiteSpace(ci)) return false;
            var patients = GetAllPatients();
            var index = patients.FindIndex(p => p.CI == ci);
            if (index == -1) return false;

            patients[index].Name = newName;
            patients[index].LastName = newLastName;
            File.WriteAllLines(filePath, patients.Select(ToLine));
            return true;
        }

        public bool DeletePatient(string ci)
        {
            if (string.IsNullOrWhiteSpace(ci) || !File.Exists(filePath)) return false;
            var patients = GetAllPatients();
            var updated = patients.Where(p => p.CI != ci).ToList();
            if (updated.Count == patients.Count) return false;

            File.WriteAllLines(filePath, updated.Select(ToLine));
            return true;
        }

        private string ToLine(PatientWithBlood p) =>
            $"{p.Name},{p.LastName},{p.CI},{p.BloodGroup}";

        private PatientWithBlood ToPatient(string line)
        {
            var parts = line.Split(',');
            return new PatientWithBlood
            {
                Name = parts[0],
                LastName = parts[1],
                CI = parts[2],
                BloodGroup = parts[3]
            };
        }

        private string GetRandomBloodGroup()
        {
            var groups = new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
            return groups[new Random().Next(groups.Length)];
        }
    }
}
