using CurrencyRateBattle.WebAPI.Abstractions;
using CurrencyRateBattle.WebAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattle.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet("balance/{id}")]
    public decimal GetBalance(int id)
    {
        return _gameService.GetBalance(id);

    }

    [HttpGet("history/{id}")]
    public Task<List<RoomUserDto>> GetHistory(int id)
    {
        return _gameService.GetHistory(id);
    }
}
