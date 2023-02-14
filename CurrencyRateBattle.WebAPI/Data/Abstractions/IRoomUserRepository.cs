using CurrencyRateBattle.WebAPI.Data.Entities;

namespace CurrencyRateBattle.WebAPI.Data.Abstractions;

public interface IRoomUserRepository : IDbRepository<RoomUser>
{

    List<RoomUser> GetRoomUserByUserId(int userId);
    List<RoomUser> GetRoomUserByRoomId(int roomId);

}
