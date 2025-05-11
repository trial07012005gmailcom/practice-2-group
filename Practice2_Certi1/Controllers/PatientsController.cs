using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using PatientManager.Models;
using PatientManager.Services;
using Services.Models;
using System;
using PatientManager.Managers;
using PatientManager.Models.DTO; 

namespace Practice2_Certi1.Controllers
{
    [ApiController]
    [Route("patients")]
    public class PatientsController : ControllerBase
    {
        private readonly ILogger<PatientsController> _logger;
        private readonly IConfiguration _config;
        private readonly PatientService _patientService;

        public PatientsController(ILogger<PatientsController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _patientService = new PatientService(); // por ahora sin DI
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("R - GET /patients called.");
            var patients = _patientService.GetAllPatients();
            return Ok(patients);
        }

        [HttpGet]
        [Route("{ci}")]
        public IActionResult GetByCI(string ci)
        {
            _logger.LogInformation($"R - GET /patients/{ci} called.");
            var patient = _patientService.GetPatientByCi(ci);
            if (patient == null)
            {
                _logger.LogWarning($"R - Patient with CI '{ci}' not found.");
                return NotFound("Patient not found");
            }

            return Ok(patient);
        }

        [HttpPost]
        [Route("")]
        public IActionResult Create([FromBody] CreatePatientDto patientDto)
        {
            
            try
            {
                var patient = new Patient
                {
                    Name = patientDto.Name,
                    LastName = patientDto.LastName,
                    CI = patientDto.CI
                };
                _logger.LogInformation($"C - POST /patients called to create CI: {patient.CI}");

                var created = _patientService.CreatePatient(patient);

                _logger.LogInformation($"C - Patient with CI '{patient.CI}' created successfully.");
                return Ok($"Patient created successfully. BloodGroup: {created.BloodGroup}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"C - Error creating patient");
                return Conflict(ex.Message);
            }
        }

        [HttpPut]
        [Route("{ci}")]
        public IActionResult Update(string ci, [FromBody] UpdatePatientDto updated)
        {
            _logger.LogInformation($"U - PUT /patients/{ci} called to update.");

            var success = _patientService.UpdatePatient(ci, updated.Name, updated.LastName);
            if (!success)
            {
                _logger.LogWarning($"U - Patient with CI '{ci}' not found for update.");
                return NotFound("Patient not found");
            }

            _logger.LogInformation($"U - Patient with CI '{ci}' updated successfully.");
            return Ok("Patient updated successfully");
        }

        [HttpDelete]
        [Route("{ci}")]
        public IActionResult Delete(string ci)
        {
            _logger.LogInformation($"D - DELETE /patients/{ci} called.");

            var success = _patientService.DeletePatient(ci);
            if (!success)
            {
                _logger.LogWarning($"D - Patient with CI '{ci}' not found for deletion.");
                return NotFound("Patient not found");
            }

            _logger.LogInformation($"D - Patient with CI '{ci}' deleted successfully.");
            return Ok("Patient deleted successfully.");
        }

        [HttpGet]
        [Route("gift")]
        public IActionResult GetGifts()
        {
            _logger.LogInformation("R - GET /patients/gift called.");

            var giftManager = new GiftManager(_config); // versión de ClinicSolution

            try
            {
                var gifts = giftManager.GetGiftsAsync().Result;
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "R - Error retrieving gifts from external API.");
                return StatusCode(500, "Failed to retrieve gifts.");
            }
        }

        [HttpPost]
        [Route("{ci}/gift")]
        public IActionResult AssignGiftToPatient(string ci)
        {
            _logger.LogInformation($"C - POST /patients/{ci}/gift called.");

            var giftManager = new GiftManager(_config);

            try
            {
                var gift = giftManager.AssignGiftToPatient(ci, _patientService).Result;

                if (gift == null)
                {
                    _logger.LogWarning($"Gift assignment failed. Patient CI '{ci}' not found or no gifts available.");
                    return NotFound("Patient not found or no gifts available.");
                }

                return Ok($"Patient with CI '{ci}' was awarded the gift '{gift.name}' with the ID '{gift.id}'.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning gift to patient.");
                return StatusCode(500, "Internal server error during gift assignment.");
            }
        }

    }
}
