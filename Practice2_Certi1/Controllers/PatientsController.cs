using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Serilog;
using Serilog.Core;
using PatientManager.Models;
using PatientManager.Services;
using System.Runtime.InteropServices;

namespace Practice2_Certi1.Controllers
{ 
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {

        private readonly PatientService patientService = new PatientService();

        // Endpoint POST /patients (crear paciente)
        [HttpPost]
        public IActionResult CreatePatient([FromBody] Patient patient)
        {
            Log.Information("Requested to create a new patient");
            try
            {
                var newPatient = patientService.CreatePatient(patient);
                Log.Information("New patient created successfully");
                return Ok($"Paciente creado con grupo sanguíneo: {newPatient.BloodGroup}");
            }
            catch (Exception ex)
            {
                Log.Error("Error al guardar el paciente: " + ex.Message);
                throw ex; 
            }
        }

        // Endpoint PUT /patients/{ci} (actualizar nombre/apellido)
        [HttpPut("{ci}")]
        public IActionResult UpdatePatient(string ci, [FromBody] Patient updatedPatient)
        {
            Log.Information("Requested to update a patient");
            try
            {
               
                bool updated = patientService.UpdatePatient(ci, updatedPatient.Name, updatedPatient.LastName);
                
                if (!updated)
                {
                    Log.Information(" UPDATE - The patient was not found"); 
                    return NotFound($"No se encontro un paciente con Ci {ci}");
                }

                Log.Information("Updated the patient successfully.");

                return Ok("Paciente actualizado exitosamente");
            }
            catch (Exception ex)
            {
                Log.Error("Error al actualizar el paciente: " + ex.Message);
                throw ex;
            }
        }

        // Endpoint DELETE /patients/{ci}
        [HttpDelete("{ci}")]
        public IActionResult DeletePatient(string ci)
        {
            Log.Information("Requested to delete a patient"); 
            try
            {
                bool deleted = patientService.DeletePatient(ci);
                

                if (!deleted)
                {
                    Log.Error("DELETE - Patient was not found"); 
                    return NotFound($"No se encontro un paciente con el CI {ci}");

                }

                Log.Information("Patient was deleated sucessfully"); 
                return Ok("Patient deleated sucessfully");

            }
            catch (Exception ex)
            {
                Log.Error("Error al eliminar el paciente: " + ex.Message);
                throw ex; 
            }
        }

        // Endpoint GET /patients
        [HttpGet]
        public IActionResult GetAllPatients()
        {
            Log.Information("Requested to get all patients by the GET endpoint");

            try
            {
                var patients = patientService.GetAllPatients();
                Log.Information("Got all patients from patients.txt sucessfully");
                return Ok(patients);
            }
            catch (Exception ex)
            {
                Log.Error("Error al leer pacientes: " + ex.Message);
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
}


