using System.ComponentModel.DataAnnotations;

namespace CurrencyRateBattle.WebAPI.Data.Entities;

public class Timing : BaseEntity
{
    [Required]
    public DateTimeOffset EndTime { get; set; }
}
