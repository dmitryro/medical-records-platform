using MedicalAPI.Models;
using MedicalAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalAPI.Controllers
{
    [ApiController]
    [Route("api/v1/doctors")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _repo;

        public DoctorController(IDoctorRepository repo)
        {
            _repo = repo;
        }

        // GET: api/v1/doctors
        [HttpGet]
        [Authorize(Policy = "ReadOnlyPolicy")]
        public async Task<IEnumerable<Doctor>> Get()
        {
            return await _repo.GetAllAsync();
        }

        // GET: api/v1/doctors/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "ReadOnlyPolicy")]
        public async Task<ActionResult<Doctor>> GetById(uint id)
        {
            var doctor = await _repo.GetByIdAsync((int)id);
            if (doctor == null)
                return NotFound();
            return doctor;
        }

        // GET: api/v1/doctors/lastname/{lastName}
        [HttpGet("lastname/{lastName}")]
        [Authorize(Policy = "ReadOnlyPolicy")]
        public async Task<IEnumerable<Doctor>> GetByLastName(string lastName)
        {
            return await _repo.GetByLastNameAsync(lastName);
        }

        // GET: api/v1/doctors/phone/{phone}
        [HttpGet("phone/{phone}")]
        [Authorize(Policy = "ReadOnlyPolicy")]
        public async Task<IEnumerable<Doctor>> GetByPhone(string phone)
        {
            return await _repo.GetByPhoneAsync(phone);
        }

        // GET: api/v1/doctors/email/{email}
        [HttpGet("email/{email}")]
        [Authorize(Policy = "ReadOnlyPolicy")]
        public async Task<ActionResult<Doctor>> GetByEmail(string email)
        {
            var doctor = await _repo.GetByEmailAsync(email);
            if (doctor == null)
                return NotFound();
            return doctor;
        }

        // GET: api/v1/doctors/specialization/{specialization}
        [HttpGet("specialization/{specialization}")]
        [Authorize(Policy = "ReadOnlyPolicy")]
        public async Task<IEnumerable<Doctor>> GetBySpecialization(string specialization)
        {
            return await _repo.GetBySpecializationAsync(specialization);
        }

        // GET: api/v1/doctors/license/{licenseNumber}
        [HttpGet("license/{licenseNumber}")]
        [Authorize(Policy = "ReadOnlyPolicy")]
        public async Task<ActionResult<Doctor>> GetByLicenseNumber(string licenseNumber)
        {
            var doctor = await _repo.GetByLicenseNumberAsync(licenseNumber);
            if (doctor == null)
                return NotFound();
            return doctor;
        }

        // POST: api/v1/doctors
        [HttpPost]
        [Authorize(Policy = "SuperuserPolicy")]
        public async Task<ActionResult<Doctor>> Post(Doctor doctor)
        {
            var created = await _repo.AddAsync(doctor);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/v1/doctors/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "SuperuserPolicy")]
        public async Task<ActionResult<Doctor>> Put(uint id, Doctor doctor)
        {
            var updated = await _repo.UpdateAsync((int)id, doctor);
            if (updated == null)
                return NotFound();
            return updated;
        }

        // DELETE: api/v1/doctors/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "SuperuserPolicy")]
        public async Task<IActionResult> Delete(uint id)
        {
            var deleted = await _repo.DeleteAsync((int)id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}

