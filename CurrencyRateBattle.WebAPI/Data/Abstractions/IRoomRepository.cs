using CurrencyRateBattle.WebAPI.Data.Entities;

namespace CurrencyRateBattle.WebAPI.Data.Abstractions;

public interface IRoomRepository : IDbRepository<Room>
{
    List<Room> GetActiveRooms();
    Task UpdateRoomSumAsync(long id, decimal cash);
}
