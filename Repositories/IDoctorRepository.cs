using MedicalAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalAPI.Repositories
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor?> GetByIdAsync(int id);
        Task<IEnumerable<Doctor>> GetByLastNameAsync(string lastName);
        Task<IEnumerable<Doctor>> GetByPhoneAsync(string phone);
        Task<Doctor?> GetByEmailAsync(string email);
        Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization);
        Task<Doctor?> GetByLicenseNumberAsync(string licenseNumber);
        Task<Doctor> AddAsync(Doctor doctor);
        Task<Doctor?> UpdateAsync(int id, Doctor doctor);
        Task<bool> DeleteAsync(int id);
    }
}

