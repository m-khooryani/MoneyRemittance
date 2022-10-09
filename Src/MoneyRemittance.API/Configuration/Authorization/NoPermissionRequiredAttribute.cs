namespace MoneyRemittance.API.Configuration.Authorization;

[AttributeUsage(AttributeTargets.Method)]
internal class NoPermissionRequiredAttribute : Attribute
{
    public NoPermissionRequiredAttribute()
        : base()
    {
    }
}
