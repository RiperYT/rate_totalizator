using CurrencyRateBattle.WebAPI.Abstractions;
using CurrencyRateBattle.WebAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattle.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BetController : ControllerBase
{
    private readonly IBetService _betService;

    public BetController(IBetService betService)
    {
        _betService = betService;
    }

    [HttpPost("betmenu")]
    public Task<BetResponseDto> Bet(UserBetDto userDto)
    {
        return _betService.MakeABet(userDto);
    }

    [HttpPost("playersinfo")]
    public Task<PlayerInfoDto> PlayersInfo(PlayerInfoDto info)
    {
        return _betService.ShowPlayers(info);
    }
}
