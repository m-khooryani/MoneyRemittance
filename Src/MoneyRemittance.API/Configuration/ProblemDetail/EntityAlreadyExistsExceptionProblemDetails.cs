using Microsoft.AspNetCore.Mvc;
using MoneyRemittance.BuildingBlocks.Application.Exceptions;

namespace MoneyRemittance.API.Configuration.ProblemDetail;

public class EntityAlreadyExistsExceptionProblemDetails : ProblemDetails
{
    public EntityAlreadyExistsExceptionProblemDetails(EntityAlreadyExistsException exception)
    {
        Title = "Duplicate Entity";
        Status = StatusCodes.Status409Conflict;
        Detail = exception.Message;
    }
}
