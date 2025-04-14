// Repositories/PatientRepository.cs
using MedicalAPI.Data;
using MedicalAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<PatientRepository> _logger;

        public PatientRepository(AppDbContext db, ILogger<PatientRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            try
            {
                return await _db.Patients.Include(p => p.MasterPatientIndex).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllAsync");
                throw;
            }
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            try
            {
                return await _db.Patients.Include(p => p.MasterPatientIndex).FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByIdAsync. Id: {Id}", id);
                throw;
            }
        }

        public async Task<Patient> AddAsync(Patient patient)
        {
            try
            {
                await _db.Patients.AddAsync(patient);
                await _db.SaveChangesAsync();
                return patient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddAsync");
                throw;
            }
        }

        public async Task<Patient?> UpdateAsync(int id, Patient patient)
        {
            try
            {
                var existing = await _db.Patients.FindAsync(id);
                if (existing == null) return null;

                existing.FirstName = patient.FirstName;
                existing.LastName = patient.LastName;
                existing.DateOfBirth = patient.DateOfBirth;
                existing.Gender = patient.Gender;
                existing.Address = patient.Address;
                existing.ContactNumber = patient.ContactNumber;
                existing.Email = patient.Email;
                existing.SocialSecurityNumber = patient.SocialSecurityNumber;
                existing.MpiId = patient.MpiId;

                await _db.SaveChangesAsync();
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateAsync. Id: {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var existing = await _db.Patients.FindAsync(id);
                if (existing == null) return false;
                _db.Patients.Remove(existing);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteAsync. Id: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Patient>> SearchByLastNameAsync(string lastName)
        {
            try
            {
                return await _db.Patients.Include(p => p.MasterPatientIndex)
                    .Where(p => p.LastName != null && p.LastName.ToLower() == lastName.ToLower())
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SearchByLastNameAsync. LastName: {LastName}", lastName);
                throw;
            }
        }

        public async Task<IEnumerable<Patient>> SearchByContactNumberAsync(string contactNumber)
        {
            try
            {
                return await _db.Patients.Include(p => p.MasterPatientIndex)
                    .Where(p => p.ContactNumber != null && p.ContactNumber.Contains(contactNumber))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SearchByContactNumberAsync. ContactNumber: {ContactNumber}", contactNumber);
                throw;
            }
        }

        public async Task<IEnumerable<Patient>> SearchByEmailAsync(string email)
        {
            try
            {
                return await _db.Patients.Include(p => p.MasterPatientIndex)
                    .Where(p => p.Email == email)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SearchByEmailAsync. Email: {Email}", email);
                throw;
            }
        }

        public async Task<Patient?> SearchBySocialSecurityNumberAsync(string socialSecurityNumber)
        {
            try
            {
                return await _db.Patients.Include(p => p.MasterPatientIndex)
                    .FirstOrDefaultAsync(p => p.SocialSecurityNumber == socialSecurityNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SearchBySocialSecurityNumberAsync. SocialSecurityNumber: {SocialSecurityNumber}", socialSecurityNumber);
                throw;
            }
        }

        public async Task<Patient?> GetByMpiIdAsync(int mpiId)
        {
            try
            {
                return await _db.Patients.Include(p => p.MasterPatientIndex)
                    .FirstOrDefaultAsync(p => p.MpiId == mpiId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByMpiIdAsync. MpiId: {MpiId}", mpiId);
                throw;
            }
        }
    }
}
