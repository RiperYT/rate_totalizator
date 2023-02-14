using System.ComponentModel.DataAnnotations;

namespace CurrencyRateBattle.WebAPI.Data.Entities;

public class Currency : BaseEntity
{
    [Required]
    public string CurrencyName { get; set; } = null!;
    [Required]
    public decimal RateActual { get; set; }
    [Required]
    public DateTimeOffset UpdateDateTime { get; set; }
}
