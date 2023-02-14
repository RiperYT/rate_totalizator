namespace CurrencyRateBattle.Client.Dtos;

public class BetResponseDto
{
    public string? Response { get; set; }

    public decimal CurrentBalance { get; set; }

    public bool IsSuccess { get; set; }
}
