using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MoneyRemittance.API.Configuration.Authentication;

namespace Microsoft.Extensions.DependencyInjection;

internal static partial class IServiceCollectionExtensions
{
    private static ILogger _logger;

    /// <summary>
    /// THIS IS JUST A SAMPLE
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddFakeAuthentication(this IServiceCollection services)
    {
        var factory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
        _logger = factory.CreateLogger("jwtAuthentication");

        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(AuthenticationConstants.Key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
               x.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = AuthenticationFailed
               };
           });

        return services;
    }

    private static Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        _logger.LogError("Authentication failed.");
        _logger.LogError(context?.Exception?.ToString());
        _logger.LogError(context?.Result?.ToString());
        return Task.CompletedTask;
    }
}
