// Controllers/PatientController.cs
using MedicalAPI.Models;
using MedicalAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalAPI.Controllers
{
    [ApiController]
    [Route("api/v1/patients")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _repo;
        private readonly ILogger<PatientController> _logger;

        public PatientController(IPatientRepository repo, ILogger<PatientController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "ReadOnlyPolicy")]
        public async Task<ActionResult<IEnumerable<Patient>>> Get()
        {
            try
            {
                var patients = await _repo.GetAllAsync();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ReadOnlyPolicy")]
        public async Task<ActionResult<Patient>> Get(int id)
        {
            try
            {
                var patient = await _repo.GetByIdAsync(id);
                if (patient == null)
                {
                    return NotFound();
                }
                return Ok(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get. Id: {Id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [Authorize(Policy = "SuperuserPolicy")]
        public async Task<ActionResult<Patient>> Post(Patient patient)
        {
            try
            {
                var createdPatient = await _repo.AddAsync(patient);
                return CreatedAtAction(nameof(Get), new { id = createdPatient.Id }, createdPatient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Post");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "SuperuserPolicy")]
        public async Task<ActionResult<Patient>> Put(int id, Patient patient)
        {
            try
            {
                var updatedPatient = await _repo.UpdateAsync(id, patient);
                if (updatedPatient == null)
                {
                    return NotFound();
                }
                return Ok(updatedPatient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Put. Id: {Id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "SuperuserPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!await _repo.DeleteAsync(id))
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Delete. Id: {Id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("search/lastname/{lastName}")]
        [Authorize(Policy = "ReadOnlyPolicy")]
        public async Task<ActionResult<IEnumerable<Patient>>> SearchByLastName(string lastName)
        {
            try
            {
                var patients = await _repo.SearchByLastNameAsync(lastName);
                return Ok(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SearchByLastName. LastName: {LastName}", lastName);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("search/contactnumber/{contactNumber}")]
        [Authorize(Policy = "ReadOnlyPolicy")]
        public async Task<ActionResult<IEnumerable<Patient>>> SearchByContactNumber(string contactNumber)
        {
            try
            {
                var patients = await _repo.SearchByContactNumberAsync(contactNumber);
                return Ok(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SearchByContactNumber. ContactNumber: {ContactNumber}", contactNumber);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("search/email/{email}")]
        [Authorize(Policy = "ReadOnlyPolicy")]
        public async Task<ActionResult<IEnumerable<Patient>>> SearchByEmail(string email)
        {
            try
            {
                var patients = await _repo.SearchByEmailAsync(email);
                return Ok(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SearchByEmail. Email: {Email}", email);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("search/ssn/{socialSecurityNumber}")]
        [Authorize(Policy = "ReadOnlyPolicy")]
        public async Task<ActionResult<Patient>> SearchBySocialSecurityNumber(string socialSecurityNumber)
        {
            try
            {
                var patient = await _repo.SearchBySocialSecurityNumberAsync(socialSecurityNumber);
                if (patient == null)
                {
                    return NotFound();
                }
                return Ok(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SearchBySocialSecurityNumber. SocialSecurityNumber: {SocialSecurityNumber}", socialSecurityNumber);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("search/mpi/{mpiId}")]
        [Authorize(Policy = "ReadOnlyPolicy")]
        public async Task<ActionResult<Patient>> GetByMpiId(int mpiId)
        {
            try
            {
                var patient = await _repo.GetByMpiIdAsync(mpiId);
                if (patient == null)
                {
                    return NotFound();
                }
                return Ok(patient);
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error in GetByMpiId. MpiId: {MpiId}", mpiId);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
