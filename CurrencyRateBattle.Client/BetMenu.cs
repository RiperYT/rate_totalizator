using System.Globalization;
using System.Text;
using System.Text.Json;
using CurrencyRateBattle.Client.Dtos;

namespace CurrencyRateBattle.Client;

internal class BetMenu
{
    private const string BaseApiUrl = "http://localhost:5124";

    public async Task<BetResponseDto> MakeABet(RoomDto room, UserDto user)
    {
        var response = new BetResponseDto();

        switch (room.TypeGame)
        {
            case TypeGame.IncreaseOrDecrease:
                Console.WriteLine($"Current rate: {room.Currency.RateActual}");
                Console.WriteLine("Write 1 if you think the rate will increase and 0 if you think the rate will decrease.");
                var ch1 = Console.ReadLine();

                if (int.TryParse(ch1, out var bet1) && bet1 is 0 or 1)
                {
                    var userBet = new UserBetDto()
                    {
                        RoomId = room.Id,
                        UserId = user.Id,
                        Bet = bet1,
                        BetTimeDate = DateTimeOffset.Now,
                        CloseAccessTime = room.CloseAccessTime,
                        Balance = user.Balance,
                        Rate = room.Rate
                    };

                    response = await GetResponseAsync<BetResponseDto>(userBet, "betmenu");
                }

                break;

            case TypeGame.ClosestValue:
                Console.WriteLine($"Current rate: {room.Currency.RateActual}");
                Console.Write("Write what course you think will be on ");
                Console.WriteLine(room.EndDateTime.DateTime);
                var ch2 = Console.ReadLine();

                ch2 = ch2.Replace(',', '.');

                if (decimal.TryParse(ch2, NumberStyles.Any, CultureInfo.InvariantCulture, out var bet2) && bet2 > 0)
                {
                    var userBet = new UserBetDto()
                    {
                        RoomId = room.Id,
                        UserId = user.Id,
                        Bet = bet2,
                        BetTimeDate = DateTimeOffset.Now,
                        CloseAccessTime = room.CloseAccessTime,
                        Balance = user.Balance,
                        Rate = room.Rate
                    };

                    response = await GetResponseAsync<BetResponseDto>(userBet, "betmenu");
                }
                break;
            default:
                break;
        }

        return response;
    }

    public async Task ShowPlayers(PlayerInfoDto info)
    {
        var newInfo = await GetResponseAsync<PlayerInfoDto>(info, "playersinfo");

        ActionHelper.PrintList(new List<PlayerInfoDto?> { newInfo });

        return;
    }

    private async Task<T> GetResponseAsync<T>(object obj, string type)
    {
        var json = JsonSerializer.Serialize(obj);

        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        var httpClient = new HttpClient();

        var responseMessage = new HttpResponseMessage();

        if (type == "betmenu")
        {
            responseMessage = await httpClient.PostAsync($"{BaseApiUrl}/api/bet/betmenu", stringContent);
        }
        else if (type == "playersinfo")
        {
            responseMessage = await httpClient.PostAsync($"{BaseApiUrl}/api/bet/playersinfo", stringContent);
        }
        var resultResponse = await responseMessage.Content.ReadAsStringAsync();

        return ConvertingHelper.Deserialize<T>(resultResponse);
    }
}
