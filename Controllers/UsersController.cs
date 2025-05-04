using MedicalAPI.Models;
using MedicalAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalAPI.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UsersController(IUserRepository repo)
        {
            _repo = repo;
        }

        // GET: api/v1/users
        [HttpGet]
        [Authorize(Policy = "ReadOnlyPolicy")] // Use ReadOnlyPolicy for read access
        public async Task<IEnumerable<User>> Get()
        {
            return await _repo.GetAllAsync();
        }

        // GET: api/v1/users/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "ReadOnlyPolicy")] // Use ReadOnlyPolicy for read access
        public async Task<ActionResult<User>> Get(uint id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            return user;
        }

        // POST: api/v1/users
        [HttpPost]
        [Authorize(Policy = "SuperuserPolicy")] // Use SuperuserPolicy for create access
        public async Task<ActionResult<User>> Post(User user)
        {
            var created = await _repo.AddAsync(user);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT: api/v1/users/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "SuperuserPolicy")] // Use SuperuserPolicy for update access
        public async Task<ActionResult<User>> Put(uint id, User user)
        {
            var updated = await _repo.UpdateAsync(id, user);
            if (updated == null)
                return NotFound();
            return updated;
        }

        // DELETE: api/v1/users/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "SuperuserPolicy")] // Use SuperuserPolicy for delete access
        public async Task<IActionResult> Delete(uint id)
        {
            if (!await _repo.DeleteAsync(id))
                return NotFound();
            return NoContent();
        }
    }
}

