using Autofac;
using MoneyRemittance.Domain.Banks.Services;
using MoneyRemittance.Domain.Beneficiaries.Services;
using MoneyRemittance.Domain.Countries.Services;
using MoneyRemittance.Domain.ExchangeRates.Services;
using MoneyRemittance.Domain.States.Services;
using MoneyRemittance.Domain.Transactions.Services;
using MoneyRemittance.Infrastructure.Domain.Banks;
using MoneyRemittance.Infrastructure.Domain.Beneficiaries;
using MoneyRemittance.Infrastructure.Domain.Countries;
using MoneyRemittance.Infrastructure.Domain.ExchangeRates;
using MoneyRemittance.Infrastructure.Domain.States;

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

        builder.RegisterType<Country>()
            .As<ICountry>()
            .SingleInstance();
        builder.RegisterType<CountryExistanceChecking>()
            .As<ICountryExistanceChecking>()
            .SingleInstance();

        builder.RegisterType<State>()
            .As<IState>()
            .SingleInstance();

        builder.RegisterType<ExchangeRate>()
            .As<IExchangeRate>()
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
