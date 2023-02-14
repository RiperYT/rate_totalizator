using System.ComponentModel.DataAnnotations;

namespace CurrencyRateBattle.WebAPI.Data.Entities;

public class Room : BaseEntity
{
    [Required]
    public int CurrencyId { get; set; }
    [Required]
    public decimal StartCurrencyRate { get; set; }
    public int? TypeId { get; set; }
    [Required]
    public decimal Rate { get; set; }
    [Required]
    public decimal Sum { get; set; } = 0;
    public decimal? EndCurrencyRate { get; set; }
    [Required]
    public DateTimeOffset EndDateTime { get; set; }
    [Required]
    public DateTimeOffset CloseAccessTime { get; set; }
}
