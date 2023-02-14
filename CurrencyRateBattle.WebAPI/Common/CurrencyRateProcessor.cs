using CurrencyRateBattle.WebAPI.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Entities;

namespace CurrencyRateBattle.WebAPI.Common;

public sealed class CurrencyRateProcessor
{
    private readonly ApiStreamProvider _apiStreamProvider;
    private readonly IJsonConverter _jsonConverter;
    private readonly ICurrencyRepository _currencyRepository;
   // private readonly IDbRepository<Currency> _dbRepository;

    public CurrencyRateProcessor(ApiStreamProvider apiStreamProvider, IJsonConverter jsonConverter, ICurrencyRepository currencyRepository)
    {
        _apiStreamProvider = apiStreamProvider;
        _jsonConverter = jsonConverter;
        _currencyRepository = currencyRepository;
    }

    public async Task<List<Currency>> GetCurrencyListAsync()
    {
        var currencyList = _currencyRepository.GetAll();

        if (!await IsStaleAsync(currencyList))
        {
            return currencyList;
        }

        var stream = await _apiStreamProvider.GetStreamAsync();
        var apiCurrencyList = await _jsonConverter.ConvertAsync<ApiCurrency>(stream);
        currencyList = apiCurrencyList.Select(x => x.ConvertToDbView()).ToList();

        await _currencyRepository.UpdateRangeCurrency(currencyList.Where(x => x.CurrencyName != null).ToList());
        await _currencyRepository.SaveChangesAsync();
        currencyList = _currencyRepository.GetAll();

        return currencyList;
    }

    private async Task<bool> IsStaleAsync(List<Currency> currencyRates)
    {
        return currencyRates.First().UpdateDateTime.AddMinutes(10).DateTime < DateTime.Now;
    }
}
