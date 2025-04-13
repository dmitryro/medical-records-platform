![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![License](https://img.shields.io/badge/license-MIT-blue)


# Medical Records Platform  - .NET Core 10 API for Electronic Medical Records platform with scheduling functionaltiy.

## Authorization and Authentication

This section describes the authorization and authentication mechanisms implemented in this API.  We use JWT (JSON Web Tokens) for authentication, role-based policies for authorization, and Bcrypt for password hashing.  We also provide user registration and login endpoints.

### Key Components

* **Packages:**
    * The following NuGet packages are used in this project:
        * `BCrypt.Net-Next`: For hashing user passwords using the Bcrypt algorithm.
            * `dotnet add package BCrypt.Net-Next`
        * `DotNetEnv`: For loading environment variables from a `.env` file.
             * `dotnet add package DotNetEnv`
        * `Microsoft.AspNetCore.Authentication.JwtBearer`: For handling JWT authentication.
            * `dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer`
        * `Microsoft.AspNetCore.Authorization`: For implementing role-based authorization.
             * `dotnet add package Microsoft.AspNetCore.Authorization`
        * `Microsoft.AspNetCore.OpenApi`:  For generating OpenAPI documentation.
            * `dotnet add package Microsoft.AspNetCore.OpenApi`
        * `Microsoft.OpenApi`:  For working with OpenAPI documents.
             * `dotnet add package Microsoft.OpenApi`
        * `Npgsql.EntityFrameworkCore.PostgreSQL`: For using PostgreSQL with Entity Framework Core.
            * `dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL`
        * `Swashbuckle.AspNetCore`: For generating Swagger UI and API documentation.
            * `dotnet add package Swashbuckle.AspNetCore`
        * `System.ComponentModel.Annotations`: For  data annotations used for model validation.
             * `dotnet add package System.ComponentModel.Annotations`

* **Imports:**
    * The following namespaces are used in the code related to authentication and authorization:
        ```csharp
        using MedicalAPI.Data;
        using MedicalAPI.Helpers;
        using MedicalAPI.Models;
        using MedicalAPI.Repositories;
        using MedicalAPI.Services;
        using Microsoft.AspNetCore.Authentication.JwtBearer;
        using Microsoft.AspNetCore.Authorization;
        using Microsoft.AspNetCore.Http;
        using Microsoft.EntityFrameworkCore;
        using Microsoft.Extensions.Configuration;
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
        using MedicalAspNetCoreAuthorization = Microsoft.AspNetCore.Authorization;
        using Microsoft.Extensions.Logging;
        ```

* **JWT Authentication:**
    * We use JWT for stateless authentication.  Upon successful login, the server issues a JWT to the client.  The client then includes this token in the `Authorization` header of subsequent requests.
    * The server validates the token's signature, expiration, and claims to authenticate the user.
    * The JWT authentication is implemented using the `Microsoft.AspNetCore.Authentication.JwtBearer` package.  Configuration is found in `Program.cs`.
    * The secret key used to sign the JWT is stored in configuration (e.g., environment variables) under `JwtSettings:SecretKey`.

* **JWT Configuration (appsettings.json):**
    * The `JwtSettings` section in `appsettings.json` (and `appsettings.Development.json`) configures the JWT authentication.  Here's an example:
    ```json
    {
      "JwtSettings": {
        "Issuer": "MedicalAPI",
        "Audience": "client-id-1,client-id-2,client-id-3",
        "SecretKey": "your-256-bit-secure-secret-key-should-be-here-32-bytes-long"
      }
    }
    ```
    * **`Issuer`**: The issuer of the JWT (i.e., your API).
    * **`Audience`**:  The intended recipients of the JWT (i.e., the client applications).  It can be a single value or a comma-separated list.
    * **`SecretKey`**:  **Crucially important!** This is the secret key used to sign the JWT.  **It must be kept absolutely secret.** A strong, random, 256-bit (32-byte) key is essential for security.  If this key is compromised, attackers can generate valid JWTs and gain unauthorized access.  **Never store the secret key in your code directly; use environment variables or a secure configuration provider.**

* **Bcrypt Password Hashing:**
    * User passwords are never stored in plain text.  We use the Bcrypt hashing algorithm (via the `BCrypt.Net` library) to securely hash passwords before storing them in the database.
    * This hashing occurs during user registration (in the `/api/auth/register` endpoint) and, if a password is being updated, in the `/api/users/{id}` endpoint.
    * Password verification during login (in the `/api/auth/login` endpoint) also uses Bcrypt.
    * **Why Bcrypt?** Bcrypt is a strong hashing algorithm that includes a "salt" (random data added to the password before hashing) and an adaptive cost factor.  The salt prevents rainbow table attacks, and the adaptive cost factor makes brute-force attacks computationally expensive.

* **Role-Based Authorization:**
    * We use role-based authorization to control access to API endpoints.  Users are assigned roles (e.g., "admin", "guest"), and these roles determine which actions they are allowed to perform.
    * Authorization policies are defined in `Program.cs`.
    * We have two main policies:
        * `ReadOnlyPolicy`:  Allows users with the "guest" or "admin" role to read data.
        * `SuperuserPolicy`:  Allows users with the "admin" role to perform all operations (create, read, update, delete).

* **Authorization Policies:**
    * `ReadOnlyPolicy`: This policy allows users with either the "guest" or "admin" role to access read-only endpoints.  This policy is applied to the `/api/users` (GET) and `/api/users/{id}` (GET), `/api/roles` (GET) and `/api/roles/{id}` (GET) endpoints.
    * `SuperuserPolicy`: This policy allows users with the "admin" role to access all endpoints, including those for creating, updating, and deleting data.  This policy is applied to the `/api/roles` (POST, PUT, DELETE) and `/api/users` (POST, PUT, DELETE) endpoints.

* **Endpoints:**
    * `/api/auth/register`:  This endpoint allows new users to register.  It takes a `RegistrationDto` (in the `Data` directory), hashes the password, creates a new `User` (in the `Models` directory), and saves it to the database.  The `RegisterService` handles the registration logic.
    * `/api/auth/login`:  This endpoint allows users to log in.  It takes a `LoginDto` (in the `Data` directory), verifies the provided username and password against the database, and, upon successful authentication, returns a JWT.  The `AuthService` handles the authentication logic.

* **Models:**
    * `User` (Models/User.cs): Represents a user in the system, including properties like `Id`, `Username`, `Password`, `Email`, `RoleId`, `First`, `Last`, and `Phone`.  The `Password` is stored as a Bcrypt hash.  The `User` also has a navigation property `Role` to represent the user's role.
    * `Role` (Models/Role.cs): Represents a user role, with properties like `Id` and `Name`.
    * `Login` (Models/Login.cs):  This model is not directly used in the current implementation.  The application uses LoginDto instead.
    * `Registration` (Models/Registration.cs): This model is not directly used in the current implementation. The application uses RegistrationDto instead.

* **DTOs (Data Transfer Objects):**
    * `LoginDto` (Data/LoginDto.cs):  A simple DTO for the login endpoint, containing `Username` and `Password`.
    * `RegistrationDto` (Data/RegistrationDto.cs): A DTO for the registration endpoint, containing user information like `Username`, `Password`, `Email`, `First`, `Last`, `Phone`, and `RoleId`.

* **Helpers:**
     * `JwtTokenGenerator` (Helpers/JwtTokenGenerator.cs): This class is responsible for generating JWT tokens.  It takes user information (including the role) and the secret key, and creates a signed JWT.

* **Authorization Attribute:**
    * `Authorization/HasPermissionAttribute.cs`: This file was not provided. If a custom authorization attribute named `HasPermissionAttribute` exists, it would likely be used to enforce fine-grained permissions beyond simple role checks.  For example, it might check if a user has a specific permission on a particular resource.  If you provide the code for this, I can include it in the description.  Without the code, I can only speculate on its general purpose.  In the current code, the application uses the built-in `[Authorize]` attribute from  `Microsoft.AspNetCore.Authorization`.

### Why This Approach?

* **Security:** Bcrypt ensures that even if the database is compromised, user passwords remain secure.  JWT provides a secure and stateless way to authenticate users.  Role-based authorization allows us to enforce access control, ensuring that users can only access the resources they are authorized to use.  A strong secret key is paramount for JWT security.
* **Scalability:** JWT's stateless nature makes it easy to scale the API.  The server doesn't need to store session information.
* **Flexibility:** Role-based authorization provides a flexible way to manage user permissions.  You can easily add new roles and permissions as your application evolves.
* **Maintainability:** Using DTOs for request payloads and a structured approach to authentication and authorization, along with configuration files, makes the code more maintainable and easier to understand.

