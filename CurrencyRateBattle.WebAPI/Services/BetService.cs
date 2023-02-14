using CurrencyRateBattle.WebAPI.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Entities;
using CurrencyRateBattle.WebAPI.Dtos;
using NLog;

namespace CurrencyRateBattle.WebAPI.Services;

public class BetService : IBetService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoomUserRepository _roomuserReposytory;

    private static Logger logger = LogManager.GetCurrentClassLogger();

    public BetService(IUserRepository userRepository, IRoomUserRepository roomuserReposytory)
    {
        _userRepository = userRepository;
        _roomuserReposytory = roomuserReposytory;
    }

    public async Task<BetResponseDto> MakeABet(UserBetDto userBet)
    {
        logger.Info("Getting list of room users by id of room {id}", userBet.RoomId);
        var roomUserList = _roomuserReposytory.GetRoomUserByRoomId(userBet.RoomId);

        if (userBet.CloseAccessTime.DateTime < DateTime.Now)
        {
            logger.Warn("User {userID} is trying to place a bet in the room {roomID} after expiring access time", userBet.UserId, userBet.RoomId);
            return new BetResponseDto()
            {
                Response = "Failed to place a bet because access time is over in this room",
                CurrentBalance = userBet.Balance,
                IsSuccess = false
            };
        }

        if (roomUserList.FirstOrDefault(x => x.UserId == userBet.UserId) != default)
        {
            logger.Warn("User {userID} is trying to place another bet in the same room {roomID}", userBet.UserId, userBet.RoomId);
            return new BetResponseDto()
            {
                Response = "Failed to place a bet because you have already placed a bet in this room",
                CurrentBalance = userBet.Balance,
                IsSuccess = false
            };
        }

        if (userBet.Balance >= userBet.Rate)
        {
            logger.Info("Withdrawing {amount} from the user's {userID} balance", userBet.Rate, userBet.UserId);
            await _userRepository.UpdateUserBalanceAsync(userBet.UserId, -userBet.Rate);
            await _userRepository.SaveChangesAsync();

            var roomUser = new RoomUser()
            {
                RoomId = userBet.RoomId,
                UserId = userBet.UserId,
                Bet = userBet.Bet,
                IsActive = true,
                BetTimeDate = userBet.BetTimeDate
            };

            logger.Info("Adding user {userID} to database", userBet.UserId);
            _ = await _roomuserReposytory.Add(roomUser);
            await _roomuserReposytory.SaveChangesAsync();

            return new BetResponseDto()
            {
                Response = "You have successfully placed a bet",
                CurrentBalance = userBet.Balance - userBet.Rate,
                IsSuccess = true
            };
        }
        else
        {
            logger.Warn("User {userID} is trying to place a bet without money in the room {roomID}", userBet.UserId, userBet.RoomId);
            return new BetResponseDto()
            {
                Response = "Failed to place a bet, there is not enough money on the balance",
                CurrentBalance = userBet.Balance,
                IsSuccess = false
            };
        }
    }

    public async Task<PlayerInfoDto> ShowPlayers(PlayerInfoDto info)
    {
        logger.Info("Getting list of room users by id of room {id}", info.RoomId);
        var playersList = _roomuserReposytory.GetRoomUserByRoomId(info.RoomId);

        return new PlayerInfoDto()
        {
            RoomId = info.RoomId,
            Usernames = playersList.Select(x => _userRepository.GetUsernameByUserID(x.UserId)).ToList(),
            Bets = playersList.Select(x => x.Bet).ToList()
        };
    }
}
