using MedicalAPI.Models;
using MedicalAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalAPI.Controllers
{
    [ApiController]
    [Route("api/v1/roles")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _repo;

        public RolesController(IRoleRepository repo)
        {
            _repo = repo;
        }

        // GET: api/v1/roles
        [HttpGet]
        [Authorize(Policy = "ReadOnlyPolicy")]
        public async Task<IEnumerable<Role>> Get()
        {
            return await _repo.GetAllAsync();
        }

        // POST: api/v1/roles
        [HttpPost]
        [Authorize(Policy = "SuperuserPolicy")]
        public async Task<ActionResult<Role>> Post(Role role)
        {
            var created = await _repo.AddAsync(role);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // GET: api/v1/roles/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "ReadOnlyPolicy")]
        public async Task<ActionResult<Role>> Get(uint id)
        {
            var role = await _repo.GetByIdAsync(id);
            if (role == null)
                return NotFound();
            return role;
        }

        // PUT: api/v1/roles/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "SuperuserPolicy")]
        public async Task<ActionResult<Role>> Put(uint id, Role role)
        {
            var updated = await _repo.UpdateAsync(id, role);
            if (updated == null)
                return NotFound();
            return updated;
        }

        // DELETE: api/v1/roles/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "SuperuserPolicy")]
        public async Task<IActionResult> Delete(uint id)
        {
            if (!await _repo.DeleteAsync(id))
                return NotFound();
            return NoContent();
        }
    }
}

