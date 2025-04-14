// Repositories/IPatientRepository.cs
using MedicalAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalAPI.Repositories
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(int id);
        Task<Patient> AddAsync(Patient patient);
        Task<Patient?> UpdateAsync(int id, Patient patient);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Patient>> SearchByLastNameAsync(string lastName);
        Task<IEnumerable<Patient>> SearchByContactNumberAsync(string contactNumber);
        Task<IEnumerable<Patient>> SearchByEmailAsync(string email);
        Task<Patient?> SearchBySocialSecurityNumberAsync(string socialSecurityNumber);
        Task<Patient?> GetByMpiIdAsync(int mpiId);
    }
}
