namespace CurrencyRateBattle.WebAPI.Dtos;

public class PlayerInfoDto
{
    public int RoomId { get; set; }

    public List<string>? Usernames { get; set; }

    public List<decimal>? Bets { get; set; }
}
