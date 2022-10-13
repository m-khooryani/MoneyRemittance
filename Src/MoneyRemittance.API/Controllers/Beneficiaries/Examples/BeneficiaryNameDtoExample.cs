﻿using MoneyRemittance.Domain.Beneficiaries.Services;
using Swashbuckle.AspNetCore.Filters;

namespace MoneyRemittance.API.Controllers.Beneficiaries.Examples;

public class BeneficiaryNameDtoExample : IExamplesProvider<BeneficiaryNameDto>
{
    public BeneficiaryNameDto GetExamples()
    {
        return new BeneficiaryNameDto("John Doe");
    }
}
