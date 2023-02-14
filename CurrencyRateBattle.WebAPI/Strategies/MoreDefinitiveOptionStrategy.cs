using CurrencyRateBattle.WebAPI.Abstractions;
using CurrencyRateBattle.WebAPI.Abstractions.Strategies;
using CurrencyRateBattle.WebAPI.Data.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Entities;
using CurrencyRateBattle.WebAPI.Data.Repositories;

namespace CurrencyRateBattle.WebAPI.Strategies;

public class MoreDefinitiveOptionStrategy : IGameStrategy
{
    private readonly IRewardSharingLogic _rewardSharingLogic;
    private readonly Data.Abstractions.ICurrencyRepository _currencyRepository;
    private readonly Data.Abstractions.IUserRepository _userRepository;
    private readonly Room _room;

    public MoreDefinitiveOptionStrategy(Data.Abstractions.IUserRepository userRepository, Data.Abstractions.ICurrencyRepository currencyRepository, IRewardSharingLogic rewardSharingLogic, Room room)
    {
        _userRepository = userRepository;
        _currencyRepository = currencyRepository;
        _rewardSharingLogic = rewardSharingLogic;
        _room = room;
    }

    public void PayReward(List<RoomUser> allPlayersList)
    {
        var (winners, losers) = GetWinnersAndLosersList(allPlayersList);
        losers.ToList().ForEach(x =>
        {
            x.IsActive = false;
            x.Win = 0;
        });

        var rewards = losers.Count <= 0
            ? _rewardSharingLogic.CalculateRewardsWithNoLosers(allPlayersList.Count * _room.Rate, winners.Count, _room.Rate)
            : _rewardSharingLogic.CalculateRewards(allPlayersList.Count * _room.Rate, winners.Count, _room.Rate);

        winners.ToList().ForEach(x =>
        {
            var reward = rewards.Pop();
            _userRepository.UpdateUserBalanceAsync(x.UserId, reward).Wait();
            x.IsActive = false;
            x.Win = reward;
        });

        return;
    }

    private (Queue<RoomUser> winners, List<RoomUser> losers) GetWinnersAndLosersList(List<RoomUser> allPlayersList)
    {
        var instrumets = new StrategyInstruments(_currencyRepository, _room);

        var roomBet = instrumets.GetBetResult(0);

        var winnersQueue = instrumets.TheClosestPlayers(allPlayersList, roomBet, instrumets.NumberOfRewards(allPlayersList.Count));
        var loserList = allPlayersList.Except(winnersQueue).ToList();

        return (winnersQueue, loserList);
    }
}
