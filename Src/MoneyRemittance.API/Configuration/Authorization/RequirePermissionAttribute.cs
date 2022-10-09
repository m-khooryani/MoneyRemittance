using Microsoft.AspNetCore.Authorization;

namespace MoneyRemittance.API.Configuration.Authorization;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
internal class RequirePermissionAttribute : AuthorizeAttribute
{
    public string Permission { get; }

    public RequirePermissionAttribute(string permission)
        : base(AuthorizationConstants.RequirePermissionPolicyName)
    {
        Permission = permission;
    }
}
