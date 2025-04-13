using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MedicalAPI.Data;
using MedicalAPI.Models;
using MedicalAPI.Repositories;

namespace MedicalAPI.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            Console.WriteLine($"=> JWT_ISSUER: {Environment.GetEnvironmentVariable("JWT_ISSUER")}");
        }

        // Authenticate and generate JWT Token
        public async Task<string?> Authenticate(LoginDto loginDto)
        {
            var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                return null;

            // Rehash password if necessary
            if (!user.Password.StartsWith("$2a$"))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(loginDto.Password);
                await _userRepository.UpdateAsync(user.Id, user);
            }

            return GenerateJwtToken(user);
        }

        // Asynchronous authentication method
        public async Task<string?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;

            // Rehash password if necessary
            if (!user.Password.StartsWith("$2a$"))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(password);
                await _userRepository.UpdateAsync(user.Id, user);
            }

            return GenerateJwtToken(user);
        }

        // Register a new user with password hashing
        public async Task<string> Register(Registration model)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(model.Username);
            if (existingUser != null)
                return "User already exists.";

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                First = model.First,
                Last = model.Last,
                Phone = model.Phone,
                RoleId = model.RoleId
            };

            var createdUser = await _userRepository.AddAsync(user);
            return $"User {createdUser.Username} registered successfully!";
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "JWT Key is not configured.");
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer", "JWT Issuer is not configured.");
            var jwtAudience = _configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience", "JWT Audience is not configured.");
            var key = Encoding.ASCII.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role?.Name ?? string.Empty)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

