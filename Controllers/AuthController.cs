using Microsoft.AspNetCore.Mvc;
using MedicalAPI.Data; // For LoginDto
using MedicalAPI.Services; // For AuthService
using System.Threading.Tasks;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "v1")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model) // Use LoginDto from MedicalAPI.Data
    {
        var token = await _authService.Authenticate(model);

        if (token == null)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new
        {
            token = token,
            expiration = DateTime.UtcNow.AddHours(1)
        });
    }
}

