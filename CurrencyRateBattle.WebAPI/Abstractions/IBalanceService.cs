using CurrencyRateBattle.WebAPI.Data.Entities;

namespace CurrencyRateBattle.WebAPI.Abstractions;

public interface IBalanceService
{
    public Task PayRewards(Room room);

    public Task GetPayment();
}
