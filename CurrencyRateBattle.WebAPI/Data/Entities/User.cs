using System.ComponentModel.DataAnnotations;

namespace CurrencyRateBattle.WebAPI.Data.Entities;

public class User : BaseEntity
{
    [Required]
    [MaxLength(15)]
    public string Login { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public decimal Balance { get; set; }
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    public DateTimeOffset RegisterDate { get; set; } = DateTimeOffset.Now;
}
