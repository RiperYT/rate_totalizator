using CurrencyRateBattle.WebAPI.Data.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Entities;

namespace CurrencyRateBattle.WebAPI.Data.Repositories;

public class RoomUserRepository : DbRepository<RoomUser>, IRoomUserRepository
{

    public RoomUserRepository(DataContext context) : base(context) { }

    public List<RoomUser> GetRoomUserByUserId(int userId)
    {
        return GetAll().Where(x => x.UserId == userId).ToList();
    }
    public List<RoomUser> GetRoomUserByRoomId(int roomId)
    {
        return GetAll().Where(x => x.RoomId == roomId).ToList();
    }

}
