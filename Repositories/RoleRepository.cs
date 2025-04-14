// Repositories/RoleRepository.cs
using MedicalAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalAPI.Data; 

namespace MedicalAPI.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _db;

        public RoleRepository(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Role>> GetAllAsync() =>
            await _db.Roles.ToListAsync();

        public async Task<Role?> GetByIdAsync(uint id) =>
            await _db.Roles.FindAsync(id);

        public async Task<Role?> GetByNameAsync(string name) =>
            await _db.Roles.FirstOrDefaultAsync(r => r.Name == name);

        public async Task<Role> AddAsync(Role role)
        {
            await _db.Roles.AddAsync(role);
            await _db.SaveChangesAsync();
            return role;
        }

        public async Task<Role?> UpdateAsync(uint id, Role role)
        {
            var existing = await _db.Roles.FindAsync(id);
            if (existing == null) return null;

            existing.Name = role.Name;
            existing.Permissions = role.Permissions;

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(uint id)
        {
            var existing = await _db.Roles.FindAsync(id);
            if (existing == null) return false;
            _db.Roles.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

