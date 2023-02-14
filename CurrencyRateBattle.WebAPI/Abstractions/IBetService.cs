using CurrencyRateBattle.WebAPI.Dtos;

namespace CurrencyRateBattle.WebAPI.Abstractions;

public interface IBetService
{
    Task<BetResponseDto> MakeABet(UserBetDto userBet);

    Task<PlayerInfoDto> ShowPlayers(PlayerInfoDto info);
}
