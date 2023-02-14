using System.ComponentModel.DataAnnotations;

namespace CurrencyRateBattle.WebAPI.Data.Entities;

public class RoomUser : BaseEntity
{
    [Required]
    public int RoomId { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public decimal Bet { get; set; }
    public decimal? Win { get; set; }
    [Required]
    public DateTimeOffset BetTimeDate { get; set; } = DateTimeOffset.Now;
}
