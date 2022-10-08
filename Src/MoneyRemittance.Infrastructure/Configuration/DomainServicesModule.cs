using Autofac;
using MoneyRemittance.Domain.Countries.Services;
using MoneyRemittance.Domain.Transactions.Services;

namespace MoneyRemittance.Infrastructure.Configuration;

public class DomainServicesModule : Module
{
    private readonly ITransactionSubmitting _transactionSubmitting;
    private readonly ICountryExistanceChecking _countryExistanceChecking;

    public DomainServicesModule(
        ITransactionSubmitting transactionSubmitting, 
        ICountryExistanceChecking countryExistanceChecking)
    {
        _transactionSubmitting = transactionSubmitting;
        _countryExistanceChecking = countryExistanceChecking;
    }

    protected override void Load(ContainerBuilder builder)
    {
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
