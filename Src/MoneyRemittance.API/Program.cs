using FluentValidation;
using HealthChecks.UI.Client;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MoneyRemittance.API.Configuration.Logging;
using MoneyRemittance.API.Configuration.ProblemDetail;
using MoneyRemittance.BuildingBlocks.Application.Exceptions;
using MoneyRemittance.BuildingBlocks.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails(problemDetailOption =>
{
    problemDetailOption.Map<ValidationException>(ex => new ValidationExceptionProblemDetails(ex));
    problemDetailOption.Map<EntityNotFoundException>(ex => new EntityNotFoundExceptionProblemDetails(ex));
    problemDetailOption.Map<EntityAlreadyExistsException>(ex => new EntityAlreadyExistsExceptionProblemDetails(ex));
    problemDetailOption.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
});
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
});
builder.Services.AddRouting(routeOption =>
{
    routeOption.LowercaseUrls = true;
});
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddFakeAuthentication();
builder.Services.AddPermissionBasedAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecksUI().AddInMemoryStorage();
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration["ConnectionString"]);
builder.Services.UseProcessorBackgroundServices();
builder.Services.AddBusinessLogicServices(builder.Configuration);

var app = builder.Build();

// HTTP request pipeline.
app.UseMiddleware<LoggingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers().RequireAuthorization();
    endpoints.MapHealthChecksUI();
    endpoints.MapHealthChecks("/health", new HealthCheckOptions()
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        AllowCachingResponses = false,
    });
});

app.Run();
