using System.Text;
using MoneyRemittance.Domain.Banks.Services;
using MoneyRemittance.Domain.Beneficiaries.Services;
using MoneyRemittance.Domain.Countries.Services;
using MoneyRemittance.Domain.ExchangeRates.Services;
using Newtonsoft.Json;

namespace MoneyRemittance.Infrastructure.Configuration.B2BApi;

internal class B2BApiClient
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly B2BApiConfig _apiConfig;

    public B2BApiClient(
        IHttpClientFactory clientFactory,
        B2BApiConfig apiConfig)
    {
        _clientFactory = clientFactory;
        _apiConfig = apiConfig;
    }

    public async Task<BankDto[]> GetBanksAsync(string country)
    {
        var client = _clientFactory.CreateClient("B2BApi");
        var content = new StringContent($"{{\"country\":\"{country}\"}}", Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{_apiConfig.Endpoint.TrimEnd('/')}/get-bank-list", content);
        return await ReadAsJsonAsync<BankDto[]>(response.Content);
    }

    public async Task<BeneficiaryNameDto> GetBeneficiaryNameAsync(string accountNumber, string bankCode)
    {
        var client = _clientFactory.CreateClient("B2BApi");
        var content = new StringContent($"{{\"accountNumber\":\"{accountNumber}\",\"bankCode\":\"{bankCode}\"}}", Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{_apiConfig.Endpoint.TrimEnd('/')}/get-beneficiary-name", content);
        return await ReadAsJsonAsync<BeneficiaryNameDto>(response.Content);
    }

    public async Task<CountryDto[]> GetCountryList()
    {
        var client = _clientFactory.CreateClient("B2BApi");
        var response = await client.PostAsync($"{_apiConfig.Endpoint.TrimEnd('/')}/get-country-list", null);
        return await ReadAsJsonAsync<CountryDto[]>(response.Content);
    }

    public async Task<ExchangeRateDto> GetExchangeRateAsync(string from, string to)
    {
        var client = _clientFactory.CreateClient("B2BApi");
        var content = new StringContent($"{{\"from\":\"{from}\",\"to\":\"{to}\"}}", Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{_apiConfig.Endpoint.TrimEnd('/')}/get-exchange-rate", content);
        return await ReadAsJsonAsync<ExchangeRateDto>(response.Content);
    }

    private static async Task<T> ReadAsJsonAsync<T>(HttpContent content)
    {
        var dataAsString = await content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(dataAsString);
    }
}
