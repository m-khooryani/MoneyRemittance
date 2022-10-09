using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace MoneyRemittance.API.Configuration.ProblemDetail;

public class ValidationExceptionProblemDetails : ProblemDetails
{
    public ValidationExceptionProblemDetails(ValidationException exception)
    {
        Title = "Invalid Request";
        Status = StatusCodes.Status400BadRequest;
        Detail = exception.Message;
    }
}
