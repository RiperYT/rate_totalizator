using CurrencyRateBattle.WebAPI.Abstractions;
using CurrencyRateBattle.WebAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattle.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpPost]
    public async Task AddRoom(CreateRoomDto roomDto)
    {
        await _roomService.AddAsync(roomDto);
    }

    [HttpGet]
    public Task<List<RoomDto?>?> GetAllRooms()
    {
        return _roomService.GetAll();
    }

    [HttpGet("{id}")]
    public Task<RoomDto?> Get(int id)
    {
        return _roomService.Get(id);
    }

    [HttpDelete("{id}")]
    public async Task Remove(int id)
    {
        await _roomService.RemoveAsync(id);
    }


}
