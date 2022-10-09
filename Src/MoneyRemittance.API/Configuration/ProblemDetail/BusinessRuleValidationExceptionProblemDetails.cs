using Microsoft.AspNetCore.Mvc;
using MoneyRemittance.BuildingBlocks.Domain;

namespace MoneyRemittance.API.Configuration.ProblemDetail;

public class BusinessRuleValidationExceptionProblemDetails : ProblemDetails
{
    public BusinessRuleValidationExceptionProblemDetails(BusinessRuleValidationException exception)
    {
        Title = "Business rule broken";
        Status = StatusCodes.Status409Conflict;
        Detail = exception.Message;
    }
}
