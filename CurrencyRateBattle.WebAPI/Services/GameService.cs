using AutoMapper;
using CurrencyRateBattle.WebAPI.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Abstractions;
using CurrencyRateBattle.WebAPI.Dtos;
using NLog;

namespace CurrencyRateBattle.WebAPI.Services;

public class GameService : IGameService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoomUserRepository _roomUserRepository;
    private readonly IMapper _mapper;
    private readonly IRoomService _roomService;
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public GameService(IUserRepository userRepository, IRoomUserRepository roomUserRepository, IMapper mapper, IRoomService roomService)
    {
        _userRepository = userRepository;
        _roomUserRepository = roomUserRepository;
        _mapper = mapper;
        _roomService = roomService;
    }

    public decimal GetBalance(int id)
    {
        _logger.Info("Getting user balance by user id '{id}'", id);
        var user = _userRepository.GetById(id);
        return user == null ? -1 : user.Balance;
    }

    public async Task<List<RoomUserDto>> GetHistory(int id)
    {
        _logger.Info("Getting list 'RoomUser' by user id '{id}'", id);
        var roomsUser = _roomUserRepository.GetRoomUserByUserId(id);

        var roomsUserDto = _mapper.Map<List<RoomUserDto>>(roomsUser);

        _logger.Info("Converting list room to list roomDto by RoomId");

        for (var i = 0; i < roomsUserDto.Count; i++)
        {
            roomsUserDto[i].Room = await _roomService.Get(roomsUser[i].RoomId);
        }

        return roomsUserDto;
    }
}
