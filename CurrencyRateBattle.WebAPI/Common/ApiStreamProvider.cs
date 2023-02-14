using CurrencyRateBattle.WebAPI.Abstractions;

namespace CurrencyRateBattle.WebAPI.Common;

public sealed class ApiStreamProvider : IStreamProvider
{
    private const string API = "https://api.monobank.ua/bank/currency";
    private readonly HttpClient _httpClient;

    public ApiStreamProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<Stream> GetStreamAsync()
    {
        return _httpClient.GetStreamAsync(API);
    }
}
