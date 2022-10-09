using MoneyRemittance.API.Controllers.Transactions.Requests;
using MoneyRemittance.Domain.Transactions;
using Swashbuckle.AspNetCore.Filters;

namespace MoneyRemittance.API.Controllers.Transactions.Examples;

public class MakeTransactionRequestExample : IExamplesProvider<MakeTransactionRequest>
{
    public MakeTransactionRequest GetExamples()
    {
        return new MakeTransactionRequest()
        {
            DateOfBirth = DateTimeOffset.Now,
            ExchangeRate = 1.02M,
            Fees = 2.5M,
            FromAmount = "FA",
            FromCurrency = "SEK",
            SenderAddress = AddressInfo.Of(
                "123456",
                "Second st.",
                "Sweden",
                "Bevaringsgatan",
                "12452",
                "Vastra"),
            SenderEmail = "some@domain.com",
            SenderFirstName = "John",
            SenderLastName = "Doe",
            ToBankAccountName = "BAN",
            ToBankAccountNumber = "123321",
            ToBankCode = "A012",
            ToBankName = "Central Bank",
            ToCountry = "Finland",
            ToFirstName = "David",
            ToLastName = "Beckham",
            TransactionNumber = "1234567890123"
        };
    }
}
