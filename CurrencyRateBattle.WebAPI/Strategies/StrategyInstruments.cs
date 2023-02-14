using CurrencyRateBattle.WebAPI.Common;
using CurrencyRateBattle.WebAPI.Data.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Entities;

namespace CurrencyRateBattle.WebAPI.Strategies;

public class StrategyInstruments
{
    private readonly ICurrencyRepository _currencyRepository;
    private readonly CurrencyRateProvider _currencyRateProvider;
    private readonly Room _room;

    public StrategyInstruments(ICurrencyRepository currencyRepository, Room room)
    {
        var jsonConverter = new JsonConverter();
        _currencyRateProvider = new CurrencyRateProvider(new CurrencyRateProcessor(new ApiStreamProvider(new HttpClient()), jsonConverter, currencyRepository), jsonConverter);

        _currencyRepository = currencyRepository;
        _room = room;
    }

    public int NumberOfRewards(int winners)
    {
        return winners == 1 ? 1 : winners is >= 2 and <= 5 ? 2 : winners >= 6 ? 5 : 0;
    }

    public decimal GetBetResult(int type)
    {
        switch (type)
        {
            case 0:
            {
                var cc = _currencyRateProvider.GetCurrencyRateByIdAsync(_room.CurrencyId).Result.CurrencyName;
                return _currencyRateProvider.GetCurrencyRateAsync(cc).Result.RateActual;
            }

            case 1:
            {
                var cc = _currencyRateProvider.GetCurrencyRateByIdAsync(_room.CurrencyId).Result.CurrencyName;
                return _room.StartCurrencyRate < _currencyRateProvider.GetCurrencyRateAsync(cc).Result.RateActual ? 1 : 0;
            }

            default:
                return default;
        }
    }

    public Queue<RoomUser> TheClosestPlayers(List<RoomUser> allPlayersList, decimal roomBet, int maxNumberOfWinners)
    {
        var winnersQueue = new Queue<RoomUser>();
        var players = allPlayersList.ToList();

        for (var i = 0; i < maxNumberOfWinners; i++)
        {
            var winner = TheClosestOne(players, roomBet);
            winnersQueue.Enqueue(winner);
            _ = players.Remove(winner);
        }

        return winnersQueue;
    }

    public bool IsCurrencyRateStable()
    {
        //var currency = _currencyRepository.GetById(_room.CurrencyId);
        return _currencyRateProvider.GetCurrencyRateByIdAsync(_room.CurrencyId).Result.RateActual == _room.StartCurrencyRate;
    }

    private RoomUser TheClosestOne(List<RoomUser> allPlayersList, decimal roomBet)
    {
        var theClosest = allPlayersList[0];

        for (var i = 0; i < allPlayersList.Count; i++)
        {
            if (Math.Abs(roomBet - theClosest.Bet) > Math.Abs(roomBet - allPlayersList[i].Bet))
            {
                theClosest = allPlayersList[i];
            }
        }

        return theClosest;
    }
}
