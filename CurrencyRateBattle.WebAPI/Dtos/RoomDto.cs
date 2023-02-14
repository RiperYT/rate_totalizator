namespace CurrencyRateBattle.WebAPI.Dtos;

public class RoomDto
{
    public int Id { get; set; }

    public CurrencyDto? Currency { get; set; }

    public DateTimeOffset EndDateTime { get; set; }

    public DateTimeOffset CloseAccessTime { get; set; }

    public TypeGame TypeGame { get; set; }

    public int CountParticipants { get; set; }

    public decimal Rate { get; set; }

    public decimal Sum { get; set; }
}
