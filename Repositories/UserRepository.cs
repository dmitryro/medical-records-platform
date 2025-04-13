using MedicalAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db) => _db = db;

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _db.Users.Include(u => u.Role).ToListAsync();

        public async Task<User?> GetByIdAsync(uint id) =>
            await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByUsernameAsync(string username) =>
            await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);

        public async Task<User> AddAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateAsync(uint id, User user)
        {
            var existing = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
            if (existing == null) return null;

            existing.First = user.First;
            existing.Last = user.Last;
            existing.RoleId = user.RoleId;
            existing.Username = user.Username;
            existing.Email = user.Email;
            existing.Password = user.Password;
            existing.Phone = user.Phone;
            existing.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<Role> GetRoleById(uint id)
        {
            return await _db.Roles.FindAsync(id);
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> DeleteAsync(uint id)
        {
            var existing = await _db.Users.FindAsync(id);
            if (existing == null) return false;
            _db.Users.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
