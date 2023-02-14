namespace CurrencyRateBattle.WebAPI.Dtos;

public class RoomUserDto
{
    public int Id { get; set; }

    public RoomDto? Room { get; set; }

    public decimal Bet { get; set; }

    public decimal? Win { get; set; }
}
