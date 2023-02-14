using System.Linq;
using CurrencyRateBattle.WebAPI.Data.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Entities;

namespace CurrencyRateBattle.WebAPI.Data.Repositories;

public class CurrencyRepository : DbRepository<Currency>, ICurrencyRepository
{

    public CurrencyRepository(DataContext context) : base(context) { }

    public Currency? GetByName(string name)
    {
        return GetAll().FirstOrDefault(c => c.CurrencyName == name);
    }

    public async Task UpdateRangeCurrency(List<Currency> currencyList)
    {
        foreach (var currency in currencyList)
        {
            var currencyToUpdate = await Task.Run(() => GetAll().FirstOrDefault(x => x.CurrencyName == currency.CurrencyName));
            if (currencyToUpdate == null)
                continue;

            currencyToUpdate.RateActual = currency.RateActual;
            currencyToUpdate.UpdateDateTime = DateTimeOffset.Now;
        }

        //await SaveChangesAsync();
    }
}
