using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Serilog;
using Serilog.Core;
using PatientManager.Models;
using PatientManager.Services;
using System.Runtime.InteropServices;
using Services.Models;
using Serilog.Sinks.SystemConsole.Themes;

namespace Practice2_Certi1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {

        private readonly PatientService patientService = new PatientService();

        // Endpoint POST /patients (crear paciente)
        [HttpPost]
        [Route("")]
        public IActionResult CreatePatient([FromBody] Patient patient)
        {
            Log.Information("Requested to create a new patient");
            try
            {
                var newPatient = patientService.CreatePatient(patient);
                Log.Information($"New patient was created successfully");
                return Ok($"New Patient with BloodGroup: {newPatient.BloodGroup}");
            }
            catch (Exception ex)
            {
                Log.Error("Error while creating new patient: " + ex.Message); 
                return StatusCode(500, "Error creating patient.");
            }
        }

        // Endpoint PUT /patients/{ci} (actualizar nombre/apellido)
        [HttpPut]
        [Route("{ci}")]
        public IActionResult UpdatePatient(string ci, [FromBody] Patient updatedPatient)
        {
            Log.Information("Requested to update a patient");
            try
            {

                bool updated = patientService.UpdatePatient(ci, updatedPatient.Name, updatedPatient.LastName);

                if (!updated)
                {
                    Log.Information(" UPDATE - The patient was not found");
                    return NotFound($"The patient with CI: {ci} wasn't found.");
                }

                Log.Information("Updated the patient successfully.");

                return Ok("Updated the patient successfully. ");
            }
            catch (Exception ex)
            {
                Log.Error("Error while updating the patient: " + ex.Message);
                return StatusCode(500, "Error updating patient.");
            }
        }

        // Endpoint DELETE /patients/{ci}
        [HttpDelete]
        [Route("{ci}")]
        public IActionResult DeletePatient(string ci)
        {
            Log.Information("Requested to delete a patient");
            try
            {
                bool deleted = patientService.DeletePatient(ci);


                if (!deleted)
                {
                    Log.Error("DELETE - Patient was not found");
                    return NotFound($"The patient with CI: {ci} wasn't found.");

                }

                Log.Information("Patient was deleted sucessfully");
                return Ok("Patient deleted sucessfully");

            }
            catch (Exception ex)
            {
                Log.Error("Error while deleting patient: " + ex.Message);
                return StatusCode(500, "Error deleting patient.");
            }
        }

        // Endpoint GET /patients
        [HttpGet]
        [Route("")]
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
                Log.Error("Error while reading patients: " + ex.Message);
                return StatusCode(500, "Error retrieving all patients.");
            }

        }

        // Endpoint GET /patients/{ci}
        [HttpGet]
        [Route("{ci}")]
        public IActionResult GetPatientByCi(string ci)
        {
            Log.Information("Requested to GET a patient by CI");
            try
            {
                var patient = patientService.GetPatientByCi(ci);

                if (patient == null)
                {
                    Log.Information($"Patient with {ci} was not found.");
                    return NotFound($"The patient with CI: {ci} wasn't found.");
                }

                Log.Information($"Patient {ci} has been found");
                return Ok(patient);
            }
            catch (Exception ex)
            {
                Log.Error("Error while searching for patient by CI: " + ex.Message);
                return StatusCode(500, "Error retrieving patient by CI.");

            }
        }

        //Endpoint POST /patients/{ci} FOR GIFTS
        [HttpPost]
        [Route("gift")]

        public IActionResult GetGift(string ci)
        {
            Log.Information("Requested to assign a gift to a patient"); 
            try
            {
                var patient = patientService.GetPatientByCi(ci);

                if (patient == null)
                {
                    Log.Information($"Patient with {ci} was not found.");
                    return NotFound($"The patient with CI: {ci} wasn't found.");
                }

                Electronic gift = patientService.AssingGiftToPatient(ci);
                Log.Information("Succesfully assinged a gift to a patient");
                return Ok($"A gift was assigned to patient: {ci}. Gift: {gift.name}");
            }
            catch (Exception ex)
            {
                Log.Error("Error whie assinging gift to a patient: " + ex.Message);
                return StatusCode(500, "Error assinging gift to patient.");
            }
        }
    }
}


