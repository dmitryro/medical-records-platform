using MedicalAPI.Data;
using MedicalAPI.Helpers;
using MedicalAPI.Models;
using MedicalAPI.Repositories;
using MedicalAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Collections.Generic;
using MedicalAPI.Authorization; // Add this using statement
using Microsoft.Extensions.Logging;

DotNetEnv.Env.Load(); // Load .env into Environment Variables

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// 1) Configure EF Core with PostgreSQL
var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
var pass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
var db = Environment.GetEnvironmentVariable("POSTGRES_DB");

var connectionString =
    $"Host={host};Port={port};Username={user};Password={pass};Database={db}";

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseNpgsql(connectionString));



// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];  // Get the secret key
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("JwtSettings:SecretKey is missing from configuration.");
}
var key = Encoding.UTF8.GetBytes(secretKey); // Convert to byte array

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key), // Use the key here
        RequireExpirationTime = true,
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            // Add this for debugging
            Console.WriteLine("Token Validated!");
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            // Add this for debugging
            Console.WriteLine($"Authentication Failed: {context.Exception.Message}");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperuserPolicy", policy =>
        policy.RequireClaim(ClaimTypes.Role, "admin"));
    options.AddPolicy("ReadOnlyPolicy", policy =>
        policy.RequireClaim(ClaimTypes.Role, "guest", "admin"));
});


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MedicalAPI", Version = "v1" });
    // Add security definition
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    // Add security requirement
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IMasterPatientIndexRepository, MasterPatientIndexRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddSingleton<JwtTokenGenerator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseDefaultFiles(); // Serve index.html
    app.UseStaticFiles();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); // Place this *before* app.MapControllers()



app.MapControllers();

// Define API endpoints
app.MapPost("/api/auth/login", async (HttpContext context, LoginDto loginDto, AuthService authService, ILogger<Program> logger) => // Added HttpContext
{
    try
    {
        var token = await authService.Authenticate(loginDto);
        if (token != null)
        {
            await context.Response.WriteAsJsonAsync(new { token }); // Use the context
            return;
        }
        else
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { message = "Unauthorized Access" }); // Use the context
            return;
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error during login");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { message = $"Internal server error: {ex.Message}" }); // Use the context
        return;
    }
}).WithName("Login").WithOpenApi();

// Logout - Invalidate token (Simplest approach - client-side)
app.MapPost("/api/auth/logout", (HttpContext context) => // Added HttpContext
{
    // In a stateless JWT authentication, the server doesn't really "log out" the user.
    // The client is responsible for discarding the token.  This endpoint simply
    // provides a convention and can be expanded if you later add server-side
    // token management (e.g., blacklisting).
    context.Response.StatusCode = 200;
    context.Response.WriteAsJsonAsync(new { message = "Logged out" });
    return;
}).WithName("Logout").WithOpenApi();


// Registration
app.MapPost("/api/auth/register", async (HttpContext context, RegistrationDto registrationDto, RegisterService registerService, ILogger<Program> logger) => // Use RegistrationDto
{
    try
    {
        var registeredUser = await registerService.RegisterUser(registrationDto);
        if (registeredUser != null)
        {
            await context.Response.WriteAsJsonAsync(registeredUser); // Use the context
            return;
        }
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new { message = "User registration failed" });  // Use the context
        return;
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error during registration");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { message = $"Internal server error: {ex.Message}" }); // Use the context
        return;
    }
}).WithName("RegisterUser").WithOpenApi();


// Roles
app.MapGet("/api/roles", [Authorize(Policy = "ReadOnlyPolicy")] async (IRoleRepository roleRepo, ILogger<Program> logger) =>
{
    try
    {
        var roles = await roleRepo.GetAllAsync();
        return Results.Ok(roles);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error getting all roles");
        return Results.Problem($"Internal server error: {ex.Message}", statusCode: 500);
    }
}).WithName("GetRoles").WithOpenApi();

app.MapGet("/api/roles/{id}", [Authorize(Policy = "ReadOnlyPolicy")] async (IRoleRepository roleRepo, uint id, ILogger<Program> logger) =>
{
    try
    {
        var role = await roleRepo.GetByIdAsync(id);
        return role != null ? Results.Ok(role) : Results.NotFound();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error getting role by ID {id}", id);
        return Results.Problem($"Internal server error: {ex.Message}", statusCode: 500);
    }
}).WithName("GetRoleById").WithOpenApi();

app.MapPost("/api/roles", [Authorize(Policy = "SuperuserPolicy")] async (IRoleRepository roleRepo, Role role, ILogger<Program> logger) =>
{
    try
    {
        var createdRole = await roleRepo.AddAsync(role);
        return Results.Created($"/api/roles/{createdRole.Id}", createdRole);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error creating role");
        return Results.Problem($"Internal server error: {ex.Message}", statusCode: 500);
    }
}).WithName("CreateRole").WithOpenApi();

app.MapPut("/api/roles/{id}", [Authorize(Policy = "SuperuserPolicy")] async (IRoleRepository roleRepo, uint id, Role role, ILogger<Program> logger) =>
{
    try
    {
        var updatedRole = await roleRepo.UpdateAsync(id, role);
        return updatedRole != null ? Results.Ok(role) : Results.NotFound();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error updating role {id}", id);
        return Results.Problem($"Internal server error: {ex.Message}", statusCode: 500);
    }
}).WithName("UpdateRole").WithOpenApi();

app.MapDelete("/api/roles/{id}", [Authorize(Policy = "SuperuserPolicy")] async (IRoleRepository roleRepo, uint id, ILogger<Program> logger) =>
{
    try
    {
        return await roleRepo.DeleteAsync(id) ? Results.NoContent() : Results.NotFound();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error deleting role {id}", id);
        return Results.Problem($"Internal server error: {ex.Message}", statusCode: 500);
    }
}).WithName("DeleteRole").WithOpenApi();

// Users
app.MapGet("/api/users", [Authorize(Policy = "ReadOnlyPolicy")] async (IUserRepository userRepo, ILogger<Program> logger) =>
{
    try
    {
        var users = await userRepo.GetAllAsync();
        return Results.Ok(users);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error getting all users");
        return Results.Problem($"Internal server error: {ex.Message}", statusCode: 500);
    }
}).WithName("GetUsers").WithOpenApi();

app.MapGet("/api/users/{id}", [Authorize(Policy = "ReadOnlyPolicy")] async (IUserRepository userRepo, uint id, ILogger<Program> logger) =>
{
    try
    {
        var user = await userRepo.GetByIdAsync(id);
        return user != null ? Results.Ok(user) : Results.NotFound();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error getting user by ID {id}", id);
        return Results.Problem($"Internal server error: {ex.Message}", statusCode: 500);
    }
}).WithName("GetUserById").WithOpenApi();

app.MapPost("/api/users", [Authorize(Policy = "SuperuserPolicy")] async (IUserRepository userRepo, User user, ILogger<Program> logger) =>
{
    try
    {
        // Hash the password before saving it.
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        var createdUser = await userRepo.AddAsync(user);
        //eager loading
        var role = await userRepo.GetRoleById((uint)createdUser.RoleId);
        createdUser.Role = role;
        return Results.Created($"/api/users/{createdUser.Id}", createdUser);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error creating user");
        return Results.Problem($"Internal server error: {ex.Message}", statusCode: 500);
    }
}).WithName("CreateUser").WithOpenApi();

app.MapPut("/api/users/{id}", [Authorize(Policy = "SuperuserPolicy")] async (IUserRepository userRepo, uint id, User user, ILogger<Program> logger) =>
{
    try
    {
        // Hash the password before updating it, if provided
        if (!string.IsNullOrEmpty(user.Password))
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        }
        var updatedUser = await userRepo.UpdateAsync(id, user);
        return updatedUser != null ? Results.Ok(updatedUser) : Results.NotFound();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error updating user {id}", id);
        return Results.Problem($"Internal server error: {ex.Message}", statusCode: 500);
    }
}).WithName("UpdateUser").WithOpenApi();

app.MapDelete("/api/users/{id}", [Authorize(Policy = "SuperuserPolicy")] async (IUserRepository userRepo, uint id, ILogger<Program> logger) =>
{
    try
    {
        return await userRepo.DeleteAsync(id) ? Results.NoContent() : Results.NotFound();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error deleting user {id}", id);
        return Results.Problem($"Internal server error: {ex.Message}", statusCode: 500);
    }
}).WithName("DeleteUser").WithOpenApi();


app.Run();
