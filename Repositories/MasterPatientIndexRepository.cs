// Models/MasterPatientIndexRepository.cs
using MedicalAPI.Data;
using MedicalAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Repositories
{
    public class MasterPatientIndexRepository : IMasterPatientIndexRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<MasterPatientIndexRepository> _logger; // Add logger

        public MasterPatientIndexRepository(AppDbContext db, ILogger<MasterPatientIndexRepository> logger) // Inject logger
        {
            _db = db;
            _logger = logger; // Store logger
        }

        public async Task<IEnumerable<MasterPatientIndex>> GetAllAsync()
        {
            try
            {
                return await _db.MasterPatientIndices.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllAsync"); // Log the error
                throw; // Re-throw the exception to be handled by the caller
            }
        }

        public async Task<MasterPatientIndex?> GetByIdAsync(int id)
        {
            try
            {
                return await _db.MasterPatientIndices.FindAsync(id);
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error in GetByIdAsync. Id: {Id}", id);
                throw;
            }
        }

        public async Task<MasterPatientIndex> AddAsync(MasterPatientIndex mpi)
        {
            try
            {
                await _db.MasterPatientIndices.AddAsync(mpi);
                await _db.SaveChangesAsync();
                return mpi;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddAsync");
                throw;
            }
        }

        public async Task<MasterPatientIndex?> UpdateAsync(int id, MasterPatientIndex mpi)
        {
            try
            {
                var existing = await _db.MasterPatientIndices.FindAsync(id);
                if (existing == null) return null;

                existing.PatientId = mpi.PatientId;
                existing.FirstName = mpi.FirstName;
                existing.LastName = mpi.LastName;
                existing.DateOfBirth = mpi.DateOfBirth;
                existing.Gender = mpi.Gender;
                existing.Address = mpi.Address;
                existing.ContactNumber = mpi.ContactNumber;
                existing.Email = mpi.Email;
                existing.SocialSecurityNumber = mpi.SocialSecurityNumber;
                existing.MatchScore = mpi.MatchScore;
                existing.MatchDate = mpi.MatchDate;
                existing.UpdatedAt = DateTime.UtcNow;

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
                var existing = await _db.MasterPatientIndices.FindAsync(id);
                if (existing == null) return false;
                _db.MasterPatientIndices.Remove(existing);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteAsync. Id: {Id}", id);
                throw;
            }
        }

        public async Task<MasterPatientIndex?> GetByPatientIdAsync(string patientId)
        {
             try
            {
                if (int.TryParse(patientId, out var parsedPatientId))
                {
                    return await _db.MasterPatientIndices.FirstOrDefaultAsync(mpi => mpi.PatientId == parsedPatientId);
                }
                return null;
            }
             catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByPatientIdAsync. PatientId: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<IEnumerable<MasterPatientIndex>> SearchByContactNumberAsync(string contactNumber)
        {
            try
            {
                return await _db.MasterPatientIndices
                    .Where(mpi => mpi.ContactNumber != null && mpi.ContactNumber.Contains(contactNumber))
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in SearchByContactNumberAsync. ContactNumber: {ContactNumber}", contactNumber);
                throw;
            }
        }

        public async Task<IEnumerable<MasterPatientIndex>> SearchByEmailAsync(string email)
        {
            try
            {
                return await _db.MasterPatientIndices
                    .Where(mpi => mpi.Email == email)
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in SearchByEmailAsync. Email: {Email}", email);
                throw;
            }
        }

        public async Task<IEnumerable<MasterPatientIndex>> SearchByLastNameAsync(string lastName)
        {
            try
            {
                return await _db.MasterPatientIndices
                    .Where(mpi => mpi.LastName.ToLower() == lastName.ToLower())
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in SearchByLastNameAsync. LastName: {LastName}", lastName);
                throw;
            }
        }

        public async Task<MasterPatientIndex?> SearchBySocialSecurityNumberAsync(string socialSecurityNumber)
        {
            try
            {
                return await _db.MasterPatientIndices
                    .FirstOrDefaultAsync(mpi => mpi.SocialSecurityNumber == socialSecurityNumber);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in SearchBySocialSecurityNumberAsync. SocialSecurityNumber: {SocialSecurityNumber}", socialSecurityNumber);
                throw;
            }
        }
    }
}

