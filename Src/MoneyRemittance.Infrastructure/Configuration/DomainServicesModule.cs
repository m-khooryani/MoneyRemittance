using Autofac;
using MoneyRemittance.Domain.Banks.Services;
using MoneyRemittance.Domain.Beneficiaries.Services;
using MoneyRemittance.Domain.Countries.Services;
using MoneyRemittance.Domain.Transactions.Services;
using MoneyRemittance.Infrastructure.Domain.Banks;
using MoneyRemittance.Infrastructure.Domain.Beneficiaries;

namespace MoneyRemittance.Infrastructure.Configuration;

public class DomainServicesModule : Module
{
    private readonly ITransactionSubmitting _transactionSubmitting;
    private readonly ICountryExistanceChecking _countryExistanceChecking;

    public DomainServicesModule()
    {
    }

    public DomainServicesModule(
        ITransactionSubmitting transactionSubmitting, 
        ICountryExistanceChecking countryExistanceChecking)
    {
        _transactionSubmitting = transactionSubmitting;
        _countryExistanceChecking = countryExistanceChecking;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<BankList>()
            .As<IBankList>()
            .SingleInstance();

        builder.RegisterType<Beneficiary>()
            .As<IBeneficiary>()
            .SingleInstance();

        if (_transactionSubmitting is not null)
        {
            builder.RegisterInstance(_transactionSubmitting)
                .As<ITransactionSubmitting>()
                .SingleInstance();
        }

        if (_countryExistanceChecking is not null)
        {
            builder.RegisterInstance(_countryExistanceChecking)
                .As<ICountryExistanceChecking>()
                .SingleInstance();
        }
    }
}
