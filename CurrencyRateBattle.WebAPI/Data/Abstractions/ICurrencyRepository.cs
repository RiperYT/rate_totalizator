using CurrencyRateBattle.WebAPI.Data.Entities;

namespace CurrencyRateBattle.WebAPI.Data.Abstractions;

public interface ICurrencyRepository : IDbRepository<Currency>
{
    Currency? GetByName(string name);

    Task UpdateRangeCurrency(List<Currency> currencyList);

}
