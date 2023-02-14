namespace CurrencyRateBattle.WebAPI.Dtos;

public class CreateRoomDto
{
    public string? CurrencyName { get; set; }

    public DateTimeOffset EndDateTime { get; set; }

    public DateTimeOffset CloseAccessTime { get; set; }

    public TypeGame TypeGame { get; set; }

    public decimal Rate { get; set; }
}
