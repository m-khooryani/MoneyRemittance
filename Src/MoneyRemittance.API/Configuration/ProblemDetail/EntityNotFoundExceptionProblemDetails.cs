using Microsoft.AspNetCore.Mvc;
using MoneyRemittance.BuildingBlocks.Application.Exceptions;

namespace MoneyRemittance.API.Configuration.ProblemDetail;

public class EntityNotFoundExceptionProblemDetails : ProblemDetails
{
    public EntityNotFoundExceptionProblemDetails(EntityNotFoundException exception)
    {
        Title = "Entity Not Found";
        Status = StatusCodes.Status404NotFound;
        Detail = exception.Message;
    }
}
