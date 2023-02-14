using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CurrencyRateBattle.WebAPI.Data.Abstractions;

namespace CurrencyRateBattle.WebAPI.Data.Entities;

public class BaseEntity : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [Required]
    public bool IsActive { get; set; } = true;
}
