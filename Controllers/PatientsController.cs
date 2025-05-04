using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Serilog;
using Serilog.Core; 

namespace Practice2_Certi1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {
        // Endpoint POST /patients (crear paciente)
        [HttpPost]
        public IActionResult CreatePatient([FromBody] PatientDto patient)
        {

            Log.Information("Requested the creation of a patient");
            if (string.IsNullOrWhiteSpace(patient.Name) || string.IsNullOrWhiteSpace(patient.LastName) || string.IsNullOrWhiteSpace(patient.CI))
            {
                Log.Information("Datos incompletos por parte del cliente"); 
                return BadRequest("Datos incompletos"); 
            }

            string[] bloodGroups ={"A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
            Random rnd = new Random();
            string randomBlood = bloodGroups[rnd.Next(bloodGroups.Length)];

            string line = $"{patient.Name},{patient.LastName},{patient.CI},{randomBlood}";
            string filePath = "patients.txt";

            try
            {
                Log.Information("Creando nuevo paciente"); 
                System.IO.File.AppendAllLines(filePath, new[] { line });
            }
            catch (Exception ex)
            {
                Log.Error("Error al guardar el paciente: " + ex.Message);
                throw ex; 
            }

            return Ok($"Paciente creado con grupo sanguineo: {randomBlood}");
            
        }

        // Endpoint PUT /patients/{ci} (actualizar nombre/apellido)
        [HttpPut("{ci}")]
        public IActionResult UpdatePatient(string ci, [FromBody] PatientDto updatedPatient)
        {
            return Ok("Paciente actualizado");
        }

        // Endpoint DELETE /patients/{ci}
        [HttpDelete("{ci}")]
        public IActionResult DeletePatient(string ci)
        {
            return Ok("Paciente eliminado");
        }

        // Endpoint GET /patients
        [HttpGet]
        public IActionResult GetAllPatients()
        {
            Log.Information("Requested to get all patients by the GET endpoint");
            string filePath = "patients.txt";
            List<PatientWithBloodDto> patients = new List<PatientWithBloodDto>();

            if (!System.IO.File.Exists(filePath))
            {
                Log.Information("Retornando lista vacia por que el archivo no existe");
                return Ok(patients);
            }

            try
            {
                Log.Debug("Intentando leer el archivo patients.txt");
                var lines = System.IO.File.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    var data = line.Split(',');

                    if (data.Length == 4)
                    {
                        patients.Add(new PatientWithBloodDto { Name = data[0], LastName = data[1], CI = data[2], BloodGroup = data[3] });
                    }
                }

                return Ok(patients);
            }
            catch (Exception ex)
            {
                Log.Error($"Error al leer pacientes: " + ex.Message);
                throw ex;
            }
            
        }

        // Endpoint GET /patients/{ci}
        [HttpGet("{ci}")]
        public IActionResult GetPatientByCi(string ci)
        {
            return Ok();
        }
    }

    // DTO temporal (lo moveremos más adelante a una clase externa)
    public class PatientDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CI { get; set; }
    }

    public class PatientWithBloodDto : PatientDto
    {
        public string BloodGroup { get; set; }
    }

}


