using MedicalAPI.Authorization; // Import the namespace containing HasPermissionAttribute
using MedicalAPI.Models;
using MedicalAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [HasPermission("superuser")] // Restrict access to users with 'superuser' permission
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UsersController(IUserRepository repo)
        {
            _repo = repo;
        }

/*
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _repo.GetAllAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(uint id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            var created = await _repo.AddAsync(user);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Put(uint id, User user)
        {
            var updated = await _repo.UpdateAsync(id, user);
            if (updated == null)
            {
                return NotFound();
            }
            return updated;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(uint id)
        {
            if (!await _repo.DeleteAsync(id))
            {
                return NotFound();
            }
            return NoContent();
        }
*/
    }
}

