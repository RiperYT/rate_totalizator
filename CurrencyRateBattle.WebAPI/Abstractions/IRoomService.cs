using CurrencyRateBattle.WebAPI.Dtos;

namespace CurrencyRateBattle.WebAPI.Abstractions;

public interface IRoomService
{
    Task AddAsync(CreateRoomDto? roomDto);

    Task RemoveAsync(int id);

    Task<RoomDto?> Get(int id);

    Task<List<RoomDto?>?> GetAll();
}
