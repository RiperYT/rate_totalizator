using CurrencyRateBattle.WebAPI.Data.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Entities;
using CurrencyRateBattle.WebAPI.Abstractions;
using CurrencyRateBattle.WebAPI.Common;
using CurrencyRateBattle.WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using CurrencyRateBattle.WebAPI.Data.Repositories;
using NLog;
using System.Diagnostics;

namespace CurrencyRateBattle.WebAPI.Services;

public class RoomCheckService
{
    private const int TimeSleep = 60000;
    private string _connectionString;
    private static bool _isActive = false;

    private static Logger _logger = LogManager.GetCurrentClassLogger();

    private IDbRepository<Timing> _dbTimingRepository;
    private IRoomRepository _roomRepository;
    private ICurrencyRepository _dbCurrencyRepository;
    private IBalanceService _balanceService;

    public RoomCheckService(string connectionString)
    {
        if (!_isActive)
        {
            _connectionString = connectionString;
            _isActive = true;
            _ = StartRoomCheckAsync();
        }
    }

    private async Task StartRoomCheckAsync()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        var options = optionsBuilder
            .UseSqlServer(_connectionString)
            .Options;
        var data = new DataContext(options);
        var roomUser = new RoomUserRepository(data);
        var room = new RoomRepository(data);
        var user = new UserRepository(data);
        var currency = new CurrencyRepository(data);
        var timing = new DbRepository<Timing>(data);


        _roomRepository = room;
        _dbCurrencyRepository = currency;
        _dbTimingRepository = timing;
        _balanceService = new BalanceService(roomUser, user, currency);
        await CheckStandart();
        await UpdateCurrencyAsync();
        await Check();
        await ListCheckAsync();
        var tr = new Thread(new ThreadStart(StartAsync));
        tr.Start();
    }

    private async Task CheckStandart()
    {
        try
        {
            _logger.Info("Start checking that all standard currencies and times are present");

            var list = _dbCurrencyRepository.GetAll();
            var datetime = DateTimeOffset.Now.AddDays(-1);

            if (!list.Any(c => c.CurrencyName == "USD"))
                _ = await _dbCurrencyRepository.Add(new Currency { CurrencyName = "USD", RateActual = 1, UpdateDateTime = datetime });
            if (!list.Any(c => c.CurrencyName == "EUR"))
                _ = await _dbCurrencyRepository.Add(new Currency { CurrencyName = "EUR", RateActual = 1, UpdateDateTime = datetime });
            if (!list.Any(c => c.CurrencyName == "PLN"))
                _ = await _dbCurrencyRepository.Add(new Currency { CurrencyName = "PLN", RateActual = 1, UpdateDateTime = datetime });
            if (!list.Any(c => c.CurrencyName == "GBP"))
                _ = await _dbCurrencyRepository.Add(new Currency { CurrencyName = "GBP", RateActual = 1, UpdateDateTime = datetime });
            if (!list.Any(c => c.CurrencyName == "CHF"))
                _ = await _dbCurrencyRepository.Add(new Currency { CurrencyName = "CHF", RateActual = 1, UpdateDateTime = datetime });

            var list1 = _dbTimingRepository.GetAll();
            datetime = new DateTime(1, 1, 1, 13, 0, 0);
            if (!list1.Any(t => t.EndTime.TimeOfDay == datetime.TimeOfDay))
                _ = await _dbTimingRepository.Add(new Timing { EndTime = datetime });

            _logger.Info("End checking that all standard currencies and times are present");

            await _dbTimingRepository.SaveChangesAsync();
            await _dbCurrencyRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Info(ex.Message);
        }
    }

    private async Task UpdateCurrencyAsync()
    {
        try
        {
            var jsonConverter = new JsonConverter();
            var currencyRateProvider = new CurrencyRateProvider(new CurrencyRateProcessor(new ApiStreamProvider(new HttpClient()), jsonConverter, _dbCurrencyRepository), jsonConverter);
            _ = await currencyRateProvider.GetCurrencyRateByIdAsync(_dbCurrencyRepository.GetAll().First().Id);
        }
        catch (Exception ex)
        {
            _logger.Info(ex.Message);
        }
    }

    private void StartAsync()
    {
        while (true)
        {
            Check().Wait();
            Thread.Sleep(TimeSleep);
        }
    }

    private async Task ListCheckAsync()
    {
        try
        {
            _logger.Info("Check the availability of all standard rooms");
            var list = _roomRepository.GetActiveRooms();
            var listTime = _dbTimingRepository.GetAll();
            var listCurrency = _dbCurrencyRepository.GetAll();
            if (list != null)
            {
                foreach (var currency in listCurrency)
                {
                    foreach (var time in listTime)
                    {
                        if (!list.Any(r => r.CurrencyId == currency.Id && r.EndDateTime.TimeOfDay == time.EndTime.TimeOfDay && r.TypeId == 0))
                        {
                            var endTime = new DateTimeOffset();
                            if (DateTime.Now.TimeOfDay < time.EndTime.TimeOfDay)
                            {
                                var d = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time.EndTime.Hour, time.EndTime.Minute, time.EndTime.Second);
                                endTime = d;
                            }
                            else
                            {
                                var d = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day, time.EndTime.Hour, time.EndTime.Minute, time.EndTime.Second);
                                endTime = d;
                            }

                            var roomNew = new Room
                            {
                                CurrencyId = (int)currency.Id,
                                IsActive = true,
                                TypeId = 0,
                                EndDateTime = endTime,
                                StartCurrencyRate = currency.RateActual,
                                Sum = 0,
                                Rate = 100,
                                CloseAccessTime = endTime.AddHours(-1)
                            };
                            _ = await _roomRepository.Add(roomNew);
                        }
                        if (!list.Any(r => r.CurrencyId == currency.Id && r.EndDateTime.TimeOfDay == time.EndTime.TimeOfDay && r.TypeId == 1))
                        {
                            var endTime = new DateTimeOffset();
                            if (DateTime.Now.TimeOfDay < time.EndTime.TimeOfDay)
                            {
                                var d = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time.EndTime.Hour, time.EndTime.Minute, time.EndTime.Second);
                                endTime = d;
                            }
                            else
                            {
                                var d = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day, time.EndTime.Hour, time.EndTime.Minute, time.EndTime.Second);
                                endTime = d;
                            }
                            Debug.WriteLine($"{currency.CurrencyName}  {time.EndTime}");

                            var roomNew = new Room
                            {
                                CurrencyId = (int)currency.Id,
                                IsActive = true,
                                TypeId = 1,
                                EndDateTime = endTime,
                                StartCurrencyRate = currency.RateActual,
                                Sum = 0,
                                Rate = 100,
                                CloseAccessTime = endTime.AddHours(-1)
                            };
                            _ = await _roomRepository.Add(roomNew);
                        }
                    }
                }
            }
            _ = await _roomRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Info(ex.Message);
        }

    }

    private async Task Check()
    {
        try
        {
            _logger.Info("Checking the relevance of the rooms");
            var list = _roomRepository.GetActiveRooms();
            var listTime = _dbTimingRepository.GetAll();
            if (list != null)
            {
                foreach (var room in list)
                {
                    if (room.EndDateTime <= DateTimeOffset.Now)
                    {
                        var p = false;
                        foreach (var time in listTime)
                        {
                            if (room.EndDateTime.TimeOfDay == time.EndTime.TimeOfDay)
                                p = true;
                        }
                        if (p)
                        {
                            var rates = _dbCurrencyRepository.GetAll();
                            if (rates == null)
                                return;
                            var rate = rates.FirstOrDefault(c => c.Id == room.CurrencyId).RateActual;
                            if (rate == null)
                                return;

                            var endTime = new DateTimeOffset();
                            if (DateTime.Now.TimeOfDay < room.EndDateTime.TimeOfDay)
                            {
                                var d = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, room.EndDateTime.Hour, room.EndDateTime.Minute, room.EndDateTime.Second);
                                endTime = d;
                            }
                            else
                            {
                                var d = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day, room.EndDateTime.Hour, room.EndDateTime.Minute, room.EndDateTime.Second);
                                endTime = d;
                            }
                            var roomNew = new Room
                            {
                                CurrencyId = room.CurrencyId,
                                Rate = room.Rate,
                                IsActive = true,
                                TypeId = room.TypeId,
                                EndDateTime = endTime,
                                StartCurrencyRate = rate,
                                Sum = 0,
                                CloseAccessTime = endTime.AddHours(-1)
                            };
                            _ = await _roomRepository.Add(roomNew);
                        }
                        await _balanceService.PayRewards(room);
                        await _roomRepository.SetActiveFalse(room.Id);
                        await _roomRepository.SaveChangesAsync();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Info(ex.Message);
        }
    }
}
