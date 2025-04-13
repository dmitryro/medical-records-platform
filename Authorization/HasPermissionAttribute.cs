using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;

namespace MedicalAPI.Authorization
{
    // Custom Authorization Attribute
    public class HasPermissionAttribute : TypeFilterAttribute
    {
        public HasPermissionAttribute(string permission) : base(typeof(HasPermissionRequirementFilter))
        {
            Arguments = new object[] { permission };
        }
    }

    public class HasPermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }
        public HasPermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }

    public class HasPermissionRequirementFilter : IAuthorizationFilter
    {
        private readonly string _permission;

        public HasPermissionRequirementFilter(string permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User == null || !context.HttpContext.User.Identity?.IsAuthenticated == false)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Get the user's roles (or permissions) from the claims.
            // **Important:** Adjust this to match how your permissions are stored in the token.
            var userRoles = context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role)
                                                          .Select(c => c.Value)
                                                          .ToList();

            // Check if the user has the required permission.
            if (!userRoles.Contains(_permission))
            {
                context.Result = new ForbidResult(); // Or UnauthorizedResult, depending on your preference
            }
        }
    }
}
