using CurrencyRateBattle.WebAPI.Data.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Entities;

namespace CurrencyRateBattle.WebAPI.Data.Repositories;

public class RoomRepository : DbRepository<Room>, IRoomRepository
{

    public RoomRepository(DataContext context) : base(context) { }

    public List<Room> GetActiveRooms()
    {
        return GetAll().Where(r => r.IsActive).ToList();
    }
    public async Task UpdateRoomSumAsync(long id, decimal cash)
    {
        var room = GetById(id);
        if (room != null)
        {
            room.Sum += cash;
            await Update(room);
        }
    }
}
