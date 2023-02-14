using CurrencyRateBattle.WebAPI.Abstractions;
using CurrencyRateBattle.WebAPI.Abstractions.Strategies;
using CurrencyRateBattle.WebAPI.Data.Entities;
using CurrencyRateBattle.WebAPI.Data.Repositories;

namespace CurrencyRateBattle.WebAPI.Strategies;

public class MoreOrLessStrategy : IGameStrategy
{
    private readonly IRewardSharingLogic _rewardSharingLogic;
    private readonly Data.Abstractions.ICurrencyRepository _currencyRepository;
    private readonly Data.Abstractions.IUserRepository _userRepository;
    private readonly Room _room;

    public MoreOrLessStrategy(Data.Abstractions.IUserRepository userRepository, Data.Abstractions.ICurrencyRepository currencyRepository, IRewardSharingLogic rewardSharingLogic, Room room)
    {
        _userRepository = userRepository;
        _currencyRepository = currencyRepository;
        _rewardSharingLogic = rewardSharingLogic;
        _room = room;
    }

    public void PayReward(List<RoomUser> allPlayersList)
    {
        var instrumets = new StrategyInstruments(_currencyRepository, _room);
        if (instrumets.IsCurrencyRateStable())
        {
            foreach (var player in allPlayersList)
            {
                _userRepository.UpdateUserBalanceAsync(player.UserId, _room.Rate).Wait();
                player.IsActive = false;
                player.Win = _room.Rate;
            }
            return;
        }
        var (winners, losers) = GetWinnersAndLosersList(allPlayersList);
        losers.ToList().ForEach(x => { x.IsActive = false; x.Win = 0; });

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

        var winnersQueue = new Queue<RoomUser>();
        var roomBet = instrumets.GetBetResult(1);

        var winnersList = allPlayersList.Where(player => player.Bet == roomBet).ToList();
        var loserList = allPlayersList.Except(winnersList).ToList();
        var numberOfRewards = winnersList.Count;

        for (var i = 0; i < numberOfRewards; i++)
        {
            var firstPlayer = winnersList.MinBy(x => x.BetTimeDate);
            winnersQueue.Enqueue(firstPlayer);
            winnersList.Remove(firstPlayer);
        }

        return (winnersQueue, loserList);
    }
}
