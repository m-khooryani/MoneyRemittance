using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MoneyRemittance.API.Configuration.Authorization;

namespace Microsoft.Extensions.DependencyInjection;

internal static partial class IServiceCollectionExtensions
{
    public static IServiceCollection AddPermissionBasedAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationConstants.RequirePermissionPolicyName, policyBuilder =>
            {
                policyBuilder.Requirements.Add(new PermissionRequirement());
                policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });
        });
        services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();

        return services;
    }
}
