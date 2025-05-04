using MedicalAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalAPI.Data;
using System.Linq;

namespace MedicalAPI.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDbContext _db;

        public DoctorRepository(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Doctor>> GetAllAsync() =>
            await _db.Doctors.ToListAsync();

        public async Task<Doctor?> GetByIdAsync(int id) =>
            await _db.Doctors.FirstOrDefaultAsync(d => d.Id == id);

        public async Task<IEnumerable<Doctor>> GetByLastNameAsync(string lastName) =>
            await _db.Doctors.Where(d => d.LastName == lastName).ToListAsync();

        public async Task<IEnumerable<Doctor>> GetByPhoneAsync(string phone) =>
            await _db.Doctors.Where(d => d.Phone == phone).ToListAsync();

        public async Task<Doctor?> GetByEmailAsync(string email) =>
            await _db.Doctors.FirstOrDefaultAsync(d => d.Email == email);

        public async Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization) =>
            await _db.Doctors.Where(d => d.Specialization == specialization).ToListAsync();

        public async Task<Doctor?> GetByLicenseNumberAsync(string licenseNumber) =>
            await _db.Doctors.FirstOrDefaultAsync(d => d.LicenseNumber == licenseNumber);

        public async Task<Doctor> AddAsync(Doctor doctor)
        {
            await _db.Doctors.AddAsync(doctor);
            await _db.SaveChangesAsync();
            return doctor;
        }

        public async Task<Doctor?> UpdateAsync(int id, Doctor doctor)
        {
            var existing = await _db.Doctors.FindAsync(id);
            if (existing == null) return null;

            existing.FirstName = doctor.FirstName;
            existing.LastName = doctor.LastName;
            existing.Phone = doctor.Phone;
            existing.Email = doctor.Email;
            existing.Specialization = doctor.Specialization;
            existing.LicenseNumber = doctor.LicenseNumber;

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _db.Doctors.FindAsync(id);
            if (existing == null) return false;
            _db.Doctors.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

