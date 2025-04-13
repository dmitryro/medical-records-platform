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

DotNetEnv.Env.Load(); // Load .env into Environment Variables

var builder = WebApplication.CreateBuilder(args);

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
app.MapPost("/api/auth/login", async (LoginDto loginDto, AuthService authService) =>
{
    var token = await authService.Authenticate(loginDto);
    if (token != null)
    {
        return Results.Ok(new { token });
    }
    else
    {
        return Results.Json(new { message = "Unauthorized Access" }, statusCode: 401); // Return 401 with JSON payload
    }
}).WithName("Login").WithOpenApi();

// Logout - Invalidate token (Simplest approach - client-side)
app.MapPost("/api/auth/logout", () =>
{
    // In a stateless JWT authentication, the server doesn't really "log out" the user.
    // The client is responsible for discarding the token.  This endpoint simply
    // provides a convention and can be expanded if you later add server-side
    // token management (e.g., blacklisting).
    return Results.Ok(new { message = "Logged out" });
}).WithName("Logout").WithOpenApi();


// Registration
app.MapPost("/api/auth/register", async (RegistrationDto registrationDto, RegisterService registerService) => // Use RegistrationDto
{
    var registeredUser = await registerService.RegisterUser(registrationDto);
    if (registeredUser != null)
    {
        return Results.Ok(registeredUser);
    }
    return Results.BadRequest(new { message = "User registration failed" });
}).WithName("RegisterUser").WithOpenApi();


// Roles
app.MapGet("/api/roles", [Authorize][HasPermission("superuser")] async (IRoleRepository roleRepo) => await roleRepo.GetAllAsync()).WithName("GetRoles").WithOpenApi();
app.MapGet("/api/roles/{id}", [Authorize][HasPermission("superuser")] async (IRoleRepository roleRepo, uint id) =>
    await roleRepo.GetByIdAsync(id) is Role role ? Results.Ok(role) : Results.NotFound()).WithName("GetRoleById").WithOpenApi();
app.MapPost("/api/roles", [Authorize][HasPermission("superuser")] async (IRoleRepository roleRepo, Role role) =>
{
    var createdRole = await roleRepo.AddAsync(role);
    return Results.Created($"/api/roles/{createdRole.Id}", createdRole);
}).WithName("CreateRole").WithOpenApi();
app.MapPut("/api/roles/{id}", [Authorize][HasPermission("superuser")] async (IRoleRepository roleRepo, uint id, Role role) =>
{
    var updatedRole = await roleRepo.UpdateAsync(id, role);
    return updatedRole != null ? Results.Ok(role) : Results.NotFound();
}).WithName("UpdateRole").WithOpenApi();
app.MapDelete("/api/roles/{id}", [Authorize][HasPermission("superuser")] async (IRoleRepository roleRepo, uint id) =>
{
    return await roleRepo.DeleteAsync(id) ? Results.NoContent() : Results.NotFound();
}).WithName("DeleteRole").WithOpenApi();

// Users
app.MapGet("/api/users", [Authorize][HasPermission("superuser")] async (IUserRepository userRepo) => await userRepo.GetAllAsync())
    .WithName("GetUsers").WithOpenApi();
app.MapGet("/api/users/{id}", [Authorize][HasPermission("superuser")] async (IUserRepository userRepo, uint id) =>
    await userRepo.GetByIdAsync(id) is User user ? Results.Ok(user) : Results.NotFound())
    .WithName("GetUserById").WithOpenApi();
app.MapPost("/api/users", [Authorize][HasPermission("superuser")] async (IUserRepository userRepo, User user) =>
{
    var createdUser = await userRepo.AddAsync(user);
    return Results.Created($"/api/users/{createdUser.Id}", createdUser);
}).WithName("CreateUser").WithOpenApi();
app.MapPut("/api/users/{id}", [Authorize][HasPermission("superuser")] async (IUserRepository userRepo, uint id, User user) =>
{
    var updatedUser = await userRepo.UpdateAsync(id, user);
    return updatedUser != null ? Results.Ok(user) : Results.NotFound();
}).WithName("UpdateUser").WithOpenApi();
app.MapDelete("/api/users/{id}", [Authorize][HasPermission("superuser")] async (IUserRepository userRepo, uint id) =>
{
    return await userRepo.DeleteAsync(id) ? Results.NoContent() : Results.NotFound();
}).WithName("DeleteUser").WithOpenApi();


app.Run();


