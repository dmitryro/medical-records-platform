using System;
using System.Threading.Tasks;
using MedicalAPI.Models;
using MedicalAPI.Repositories;
using MedicalAPI.Data;
using BCrypt.Net;

namespace MedicalAPI.Services
{
    public class RegisterService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public RegisterService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<User?> RegisterUser(RegistrationDto registrationDto) // Changed return type to User?
        {
            // Check if a user already exists with the provided username
            var existingUser = await _userRepository.GetByUsernameAsync(registrationDto.Username);
            if (existingUser != null)
                return null; // User already exists with the same username

            // Fetch the role based on RoleId provided in registration
            var role = await _roleRepository.GetByIdAsync(registrationDto.RoleId);
            if (role == null)
                return null; // Invalid role ID, no such role exists

            // Hash the password before saving
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password);

            // Create a new user and assign the role via RoleId
            var user = new User
            {
                First = registrationDto.First,
                Last = registrationDto.Last,
                Username = registrationDto.Username,
                Email = registrationDto.Email,
                Password = hashedPassword,
                Phone = registrationDto.Phone,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                RoleId = registrationDto.RoleId,
                Role = role
            };

            // Save the user to the database
            var createdUser = await _userRepository.AddAsync(user); // Capture the returned user
            return createdUser; // Return the created user
        }
    }
}

