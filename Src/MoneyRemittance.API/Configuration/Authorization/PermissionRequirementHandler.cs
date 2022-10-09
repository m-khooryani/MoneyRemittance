using Microsoft.AspNetCore.Authorization;

namespace MoneyRemittance.API.Configuration.Authorization;

internal class PermissionRequirementHandler
        : AuthorizationHandler<PermissionRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<PermissionRequirementHandler> _logger;

    public PermissionRequirementHandler(
        IHttpContextAccessor httpContextAccessor,
        ILogger<PermissionRequirementHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        _logger.LogInformation("Checking Authorization...");
        if (!context.User.Identity.IsAuthenticated)
        {
            _logger.LogWarning("User is not authenticated. skipping authorization");
            return;
        }

        // get [RequirePermission]
        var attribute = _httpContextAccessor
                .HttpContext
                .GetEndpoint().Metadata.GetMetadata<RequirePermissionAttribute>();

        // does the user have the permission for the endpoint?
        if (!UserHasThePermission(attribute.Permission))
        {
            _logger.LogWarning("no access to the endpoint!");
            context.Fail();
            return;
        }

        // has access
        _logger.LogInformation("have access to the endpoint");
        context.Succeed(requirement);
        await Task.CompletedTask;
    }

    private static bool UserHasThePermission(string permission)
    {
        // real logic here
        // call identity service for
        // getting user permissions

        return true;
    }
}
