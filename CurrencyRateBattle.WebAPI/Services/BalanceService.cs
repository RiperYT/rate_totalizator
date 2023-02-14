using CurrencyRateBattle.WebAPI.Abstractions;
using CurrencyRateBattle.WebAPI.Abstractions.Strategies;
using CurrencyRateBattle.WebAPI.Common;
using CurrencyRateBattle.WebAPI.Data.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Entities;
using CurrencyRateBattle.WebAPI.Strategies;
using NLog;

namespace CurrencyRateBattle.WebAPI.Services;

public class BalanceService : IBalanceService
{
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoomUserRepository _roomUserRepository;
    private IGameStrategy _strategy;
    private Room _room;

    private static Logger logger = LogManager.GetCurrentClassLogger();

    public BalanceService(IRoomUserRepository roomUserRepository, IUserRepository userRepository, ICurrencyRepository currencyRepository)
    {
        _userRepository = userRepository;
        _roomUserRepository = roomUserRepository;
        _currencyRepository = currencyRepository;
    }

    public Task GetPayment()
    {
        throw new NotImplementedException();
    }

    public async Task PayRewards(Room room)
    {
        _room = room;
        _strategy = GetStrategy(room);
        logger.Info("Getting list of room users by id of room {id}", _room.Id);
        var allPlayers = _roomUserRepository.GetRoomUserByRoomId((int)_room.Id);

        switch (allPlayers.Count)
        {
            case <= 0:
                return;
            case 1:
                logger.Info("Return of rate to the user {userID} due to a non-played room {roomID}", allPlayers[0].UserId, _room.Id);
                await _userRepository.UpdateUserBalanceAsync(allPlayers[0].UserId, _room.Rate);
                //await _userRepository.SaveChangesAsync();
                allPlayers[0].IsActive = false;
                allPlayers[0].Win = _room.Rate;
                break;
            default:

                _strategy.PayReward(allPlayers);
                break;
        }
        await _roomUserRepository.UpdateRange(allPlayers);
        //await _roomUserRepository.SaveChangesAsync();
        return;
    }

    private IGameStrategy GetStrategy(Room room)
    {
        return room.TypeId == 0
            ? new MoreOrLessStrategy(_userRepository, _currencyRepository, new RewardSharingLogic(), room)
            : new MoreDefinitiveOptionStrategy(_userRepository, _currencyRepository, new RewardSharingLogic(), room);
    }
}
