using CurrencyRateBattle.WebAPI.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Entities;

namespace CurrencyRateBattle.WebAPI.Common;

public class CurrencyRateProvider
{
    private readonly CurrencyRateProcessor _currencyProcessor;
    private readonly IJsonConverter _jsonConverter;

    public CurrencyRateProvider(CurrencyRateProcessor currencyProcessor, IJsonConverter jsonConverter)
    {
        _currencyProcessor = currencyProcessor;
        _jsonConverter = jsonConverter;
    }

    public async Task<Currency> GetCurrencyRateAsync(string cc)
    {
        var currencyRate = await _currencyProcessor.GetCurrencyListAsync();
        return currencyRate.First(n => string.Equals(n.CurrencyName, cc, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<Currency> GetCurrencyRateByIdAsync(long id)
    {
        var currencyRate = await _currencyProcessor.GetCurrencyListAsync();
        return currencyRate.First(n => n.Id == id);
    }
}
