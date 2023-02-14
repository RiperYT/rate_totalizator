using CurrencyRateBattle.WebAPI.Data.Entities;

namespace CurrencyRateBattle.WebAPI.Abstractions.Strategies;

public interface IGameStrategy
{
    void PayReward(List<RoomUser> allPlayersList);
}
