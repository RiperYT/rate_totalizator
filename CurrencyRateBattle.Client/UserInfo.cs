using CurrencyRateBattle.Client.Dtos;

namespace CurrencyRateBattle.Client;

public class UserInfo
{
    private readonly HttpClient _httpClient = new();
    private readonly string _baseApiUrl = "http://localhost:5124/api";
    public async Task<List<RoomUserDto?>?> GetHistory(int id)
    {
        var response = await GetAsync($"/game/history/{id}");

        var history = ConvertingHelper.Deserialize<List<RoomUserDto?>>(response);

        return history;
    }

    public async Task<double> GetBalance(int id)
    {
        var response = await GetAsync($"/game/balance/{id}");

        var balance = ConvertingHelper.Deserialize<double>(response);

        return balance;
    }

    public async Task<List<RoomDto?>?> GetAllRooms()
    {
        var response = await GetAsync($"/rooms");

        var user = ConvertingHelper.Deserialize<List<RoomDto?>>(response);

        return user;
    }

    private async Task<string?> GetAsync(string url)
    {
        var responseMessage = await _httpClient.GetAsync($"{_baseApiUrl}{url}");
        var response = await responseMessage.Content.ReadAsStringAsync();

        return response;
    }
}
