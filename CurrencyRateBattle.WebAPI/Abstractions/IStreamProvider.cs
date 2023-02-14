namespace CurrencyRateBattle.WebAPI.Abstractions;

public interface IStreamProvider
{
    Task<Stream> GetStreamAsync();
}
