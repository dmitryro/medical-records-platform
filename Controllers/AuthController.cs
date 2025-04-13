using Microsoft.AspNetCore.Mvc;
using MedicalAPI.Data; // Include the Data namespace for LoginDto
using MedicalAPI.Services; // Ensure this is included to use AuthService
using MedicalAPI.Models;

[ApiController]
[Route("api/[Controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly IConfiguration _configuration;

    public AuthController(AuthService authService, IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
    }
/*
    [HttpPost("auth/register")]
    public async Task<IActionResult> Register([FromBody] Registration model)
    {
        var result = await _authService.Register(model);

        if (result == null)
            return BadRequest("User already exists.");

        return Ok(new { message = result });
    }

    [HttpPost("auth/login")]
    public IActionResult Login([FromBody] LoginDto model) // Use LoginDto from MedicalAPI.Data
    {
        var token = _authService.Authenticate(model);

        if (token == null)
            return Unauthorized();

        return Ok(new
        {
            token = token,
            expiration = DateTime.UtcNow.AddHours(1)
        });
    }
*/
}

