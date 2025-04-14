// Models/IMasterPatientIndexRepository.cs
using MedicalAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalAPI.Repositories
{
    public interface IMasterPatientIndexRepository
    {
        Task<IEnumerable<MasterPatientIndex>> GetAllAsync();
        Task<MasterPatientIndex?> GetByIdAsync(int id);
        Task<MasterPatientIndex> AddAsync(MasterPatientIndex mpi);
        Task<MasterPatientIndex?> UpdateAsync(int id, MasterPatientIndex mpi);
        Task<bool> DeleteAsync(int id);
        Task<MasterPatientIndex?> GetByPatientIdAsync(string patientId);
        Task<IEnumerable<MasterPatientIndex>> SearchByContactNumberAsync(string phoneNumber);
        Task<IEnumerable<MasterPatientIndex>> SearchByEmailAsync(string email);
        Task<IEnumerable<MasterPatientIndex>> SearchByLastNameAsync(string lastName);
        Task<MasterPatientIndex?> SearchBySocialSecurityNumberAsync(string socialSecurityNumber);
        // You might want to add other specific query methods here,
        // for example, searching by first/last name, date of birth, etc.
    }
}
