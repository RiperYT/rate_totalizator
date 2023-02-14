namespace CurrencyRateBattle.Client.Dtos;

public class CurrencyDto
{
    public int Id { get; set; }

    public string? CurrencyName { get; set; }

    public decimal RateActual { get; set; }
}
