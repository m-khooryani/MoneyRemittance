using System.Text;
using MoneyRemittance.Domain.Banks.Services;
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
        //_logger.LogInformation("account service called.");
        return await ReadAsJsonAsync<BankDto[]>(response.Content);
    }

    private static async Task<T> ReadAsJsonAsync<T>(HttpContent content)
    {
        var dataAsString = await content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(dataAsString);
    }
}
