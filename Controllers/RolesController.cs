using MedicalAPI.Models;
using MedicalAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _repo;
        public RolesController(IRoleRepository repo) => _repo = repo;
/*
        [HttpGet]
        public async Task<IEnumerable<Role>> Get() =>
            await _repo.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> Get(uint id)
        {
            var role = await _repo.GetByIdAsync(id);
            if (role == null) return NotFound();
            return role;
        }

        [HttpPost]
        public async Task<ActionResult<Role>> Post(Role role)
        {
            var created = await _repo.AddAsync(role);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Role>> Put(uint id, Role role)
        {
            var updated = await _repo.UpdateAsync(id, role);
            if (updated == null) return NotFound();
            return updated;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(uint id)
        {
            if (!await _repo.DeleteAsync(id)) return NotFound();
            return NoContent();
        }
*/
    }
}

