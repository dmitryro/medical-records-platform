// Repositories/IUserRepository.cs
using MedicalAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalAPI.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(uint id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> AddAsync(User user);
        Task<User?> UpdateAsync(uint id, User user);
        Task<bool> DeleteAsync(uint id);
        Task<User> GetUserByUsername(string username);
        Task<Role> GetRoleById(uint id); 
    }
}

