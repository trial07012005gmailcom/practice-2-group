using Microsoft.AspNetCore.Mvc;

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
            return Ok("Paciente creado");
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
            return Ok();
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
}
