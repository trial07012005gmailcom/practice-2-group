using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Serilog;
using Serilog.Core;
using PatientManager.Models;
using PatientManager.Services;

namespace Practice2_Certi1.Controllers
{ 
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {

        private readonly PatientService _patientService = new PatientService();

        // Endpoint POST /patients (crear paciente)
        [HttpPost]
        public IActionResult CreatePatient([FromBody] Patient patient)
        {
            try
            {
                var newPatient = _patientService.CreatePatient(patient);
                return Ok($"Paciente creado con grupo sanguíneo: {newPatient.BloodGroup}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al guardar el paciente");
                return StatusCode(500, "Error interno al guardar paciente.");
            }
        }

        // Endpoint PUT /patients/{ci} (actualizar nombre/apellido)
        [HttpPut("{ci}")]
        public IActionResult UpdatePatient(string ci, [FromBody] Patient updatedPatient)
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

            try
            {
                var patients = _patientService.GetAllPatients();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al leer pacientes");
                return StatusCode(500, "Error interno al leer los pacientes.");
            }

        }

        // Endpoint GET /patients/{ci}
        [HttpGet("{ci}")]
        public IActionResult GetPatientByCi(string ci)
        {
            return Ok();
        }
    }
}


