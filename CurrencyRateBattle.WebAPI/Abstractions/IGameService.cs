using CurrencyRateBattle.WebAPI.Dtos;

namespace CurrencyRateBattle.WebAPI.Abstractions;

public interface IGameService
{
    decimal GetBalance(int id);

    Task<List<RoomUserDto>> GetHistory(int id);
}
