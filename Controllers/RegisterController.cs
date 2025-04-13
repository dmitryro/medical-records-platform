using Microsoft.AspNetCore.Mvc;
using MedicalAPI.Models;
using MedicalAPI.Services;
using MedicalAPI.Data;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class RegisterController : ControllerBase
{
    private readonly RegisterService _registerService;

    public RegisterController(RegisterService registerService)
    {
        _registerService = registerService;
    }
/*
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] Registration model)
    {
        if (model == null)
        {
            return BadRequest(new { message = "Invalid data." });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Validation failed.", details = ModelState });
        }

        // Convert Registration model to RegistrationDto
        var registrationDto = new RegistrationDto
        {
            First = model.First,
            Last = model.Last,
            Username = model.Username,
            Email = model.Email,
            Password = model.Password,
            Phone = model.Phone,
            RoleId = model.RoleId
        };

        var registeredUser = await _registerService.RegisterUser(registrationDto); // Changed to User?

        if (registeredUser != null)
        {
            return Ok(new { message = "User registered successfully.", user = registeredUser }); // Return user data
        }

        return BadRequest(new { message = "Username already exists or role missing." });
    }

*/
}

