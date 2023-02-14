using AutoMapper;
using CurrencyRateBattle.WebAPI.Abstractions;
using CurrencyRateBattle.WebAPI.Common;
using CurrencyRateBattle.WebAPI.Data.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Entities;
using CurrencyRateBattle.WebAPI.Dtos;
using NLog;

namespace CurrencyRateBattle.WebAPI.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IRoomUserRepository _roomUserRepository;
    private readonly IMapper _mapper;

    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    public RoomService(IRoomRepository roomRepository, ICurrencyRepository currencyRepository, IRoomUserRepository userRoomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _currencyRepository = currencyRepository;
        _roomUserRepository = userRoomRepository;
        _mapper = mapper;
    }

    public async Task AddAsync(CreateRoomDto? roomDto)
    {
        _logger.Info("Validate input data to create room");
        if (roomDto == null || roomDto.CurrencyName == null || !IsActive(roomDto.EndDateTime, roomDto.CloseAccessTime))
        {
            _logger.Warn("Invalid input data");
            return;
        }

        _logger.Info($"Getting currency by currency name '{roomDto.CurrencyName}'");
        var currency = _currencyRepository.GetByName(roomDto.CurrencyName);

        if (currency == null)
        {
            _logger.Warn("Currency with name '{CurrencyName} not found'", roomDto.CurrencyName);
            return;
        }

        var room = _mapper.Map<Room>(roomDto);


        room.CurrencyId = (int)currency.Id;
        room.StartCurrencyRate = currency.RateActual;

        _logger.Info("Write user to database");
        _ = await _roomRepository.Add(room);
        await _roomRepository.SaveChangesAsync();
    }

    public async Task<List<RoomDto?>?> GetAll()
    {
        _logger.Info($"Getting list rooms");
        var rooms = _roomRepository.GetActiveRooms();
        if (rooms == null)
        {
            _logger.Warn($"There are no such room number at the moment");
            return null;
        }

        var roomsDto = _mapper.Map<List<RoomDto?>>(rooms);

        _logger.Info($"Getting list roomsDto by rooms id");
        for (var i = 0; i < roomsDto.Count; i++)
        {
            roomsDto[i] = await Get((int)rooms[i].Id);
        }

        return roomsDto;
    }

    public async Task<RoomDto?> Get(int id)
    {
        _logger.Info("Getting room by room id '{id}'", id);
        var room = _roomRepository.GetById(id);

        if (room == null)
        {
            _logger.Warn("Room with id '{id}' wasn`t found", id);
            return null;
        }


        var jsonConverter = new JsonConverter();
        var currencyRateProvider = new CurrencyRateProvider(new CurrencyRateProcessor(new ApiStreamProvider(new HttpClient()), jsonConverter, _currencyRepository), jsonConverter);
        var currency = await currencyRateProvider.GetCurrencyRateByIdAsync(room.CurrencyId);

        var currencyDto = _mapper.Map<CurrencyDto>(currency);
        currencyDto.Id = room.CurrencyId;

        var roomDto = _mapper.Map<RoomDto>(room);

        _logger.Info("Getting roomUser by room id '{room.Id}'", room.Id);
        var usersRooms = _roomUserRepository.GetRoomUserByRoomId((int)room.Id);

        if (usersRooms == null)
            return roomDto;

        roomDto.Currency = currencyDto;
        roomDto.CountParticipants = usersRooms.Count;
        roomDto.Sum = roomDto.Rate * roomDto.CountParticipants;

        return roomDto;
    }

    public async Task RemoveAsync(int id)
    {
        _logger.Info("Remove user with id '{id}' from database", id);
        await _roomRepository.SetActiveFalse(id);
        await _roomRepository.SaveChangesAsync();
    }

    private bool IsActive(DateTimeOffset endTime, DateTimeOffset closeTime)
    {
        return DateTimeOffset.UtcNow < endTime && endTime > closeTime;
    }
}
