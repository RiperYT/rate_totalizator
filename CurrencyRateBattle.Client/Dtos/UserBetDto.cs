namespace CurrencyRateBattle.Client.Dtos;

public class UserBetDto
{
    public int RoomId { get; set; }

    public int UserId { get; set; }

    public decimal Bet { get; set; }

    public DateTimeOffset BetTimeDate { get; set; }

    public DateTimeOffset CloseAccessTime { get; set; }

    public decimal Balance { get; set; }

    public decimal Rate { get; set; }
}
