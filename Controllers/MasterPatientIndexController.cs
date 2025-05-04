// Controllers/MasterPatientIndexController.cs
using MedicalAPI.Authorization; // Import the namespace containing HasPermissionAttribute
using MedicalAPI.Models;
using MedicalAPI.Repositories;
using Microsoft.AspNetCore.Authorization; // Import this
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalAPI.Controllers
{
    [ApiController]
    [Route("api/v1/mpi")]
    [ApiExplorerSettings(GroupName = "v1")]
    //[HasPermission("superuser")] // Remove this - we'll use policies
    [Authorize] //  Require authorization for all actions in this controller
    public class MasterPatientIndexController : ControllerBase
    {
        private readonly IMasterPatientIndexRepository _repo;

        public MasterPatientIndexController(IMasterPatientIndexRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Authorize(Policy = "ReadOnlyPolicy")] // Apply ReadOnlyPolicy
        public async Task<ActionResult<IEnumerable<MasterPatientIndex>>> Get()
        {
            var mpis = await _repo.GetAllAsync();
            return Ok(mpis);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ReadOnlyPolicy")]  // Apply ReadOnlyPolicy
        public async Task<ActionResult<MasterPatientIndex>> Get(int id)
        {
            var mpi = await _repo.GetByIdAsync(id);
            if (mpi == null)
            {
                return NotFound();
            }
            return Ok(mpi);
        }

        [HttpPost]
        [Authorize(Policy = "SuperuserPolicy")] // Apply SuperuserPolicy
        public async Task<ActionResult<MasterPatientIndex>> Post(MasterPatientIndex mpi)
        {
            var createdMpi = await _repo.AddAsync(mpi);
            return CreatedAtAction(nameof(Get), new { id = createdMpi.Id }, createdMpi);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "SuperuserPolicy")] // Apply SuperuserPolicy
        public async Task<ActionResult<MasterPatientIndex>> Put(int id, MasterPatientIndex mpi)
        {
            var updatedMpi = await _repo.UpdateAsync(id, mpi);
            if (updatedMpi == null)
            {
                return NotFound();
            }
            return Ok(updatedMpi);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "SuperuserPolicy")] // Apply SuperuserPolicy
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _repo.DeleteAsync(id))
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("patient/{patientId}")]
        [Authorize(Policy = "ReadOnlyPolicy")] // Apply ReadOnlyPolicy
        public async Task<ActionResult<MasterPatientIndex>> GetByPatientId(string patientId)
        {
            var mpi = await _repo.GetByPatientIdAsync(patientId);
            if (mpi == null)
            {
                return NotFound();
            }
            return Ok(mpi);
        }

        [HttpGet("search/phone/{contactNumber}")]
        [Authorize(Policy = "ReadOnlyPolicy")]  // Apply ReadOnlyPolicy
        public async Task<ActionResult<IEnumerable<MasterPatientIndex>>> SearchByContactNumber(string contactNumber)
        {
            var results = await _repo.SearchByContactNumberAsync(contactNumber);
            return Ok(results);
        }

        [HttpGet("search/email/{email}")]
        [Authorize(Policy = "ReadOnlyPolicy")] // Apply ReadOnlyPolicy
        public async Task<ActionResult<IEnumerable<MasterPatientIndex>>> SearchByEmail(string email)
        {
            var results = await _repo.SearchByEmailAsync(email);
            return Ok(results);
        }

        [HttpGet("search/lastname/{lastName}")]
        [Authorize(Policy = "ReadOnlyPolicy")] // Apply ReadOnlyPolicy
        public async Task<ActionResult<IEnumerable<MasterPatientIndex>>> SearchByLastName(string lastName)
        {
            var results = await _repo.SearchByLastNameAsync(lastName);
            return Ok(results);
        }

        [HttpGet("search/ssn/{socialSecurityNumber}")]
        [Authorize(Policy = "ReadOnlyPolicy")] // Apply ReadOnlyPolicy
        public async Task<ActionResult<MasterPatientIndex>> SearchBySocialSecurityNumber(string socialSecurityNumber)
        {
            var result = await _repo.SearchBySocialSecurityNumberAsync(socialSecurityNumber);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
