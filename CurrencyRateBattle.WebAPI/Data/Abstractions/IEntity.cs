namespace CurrencyRateBattle.WebAPI.Data.Abstractions;

public interface IEntity
{
    long Id { get; set; }
    bool IsActive { get; set; }
}
