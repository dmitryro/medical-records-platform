// Repositories/IRoleRepository.cs
using MedicalAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalAPI.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(uint id);
        Task<Role?> GetByNameAsync(string name); // Added for retrieving roles by name
        Task<Role> AddAsync(Role role);
        Task<Role?> UpdateAsync(uint id, Role role);
        Task<bool> DeleteAsync(uint id);
    }
}
