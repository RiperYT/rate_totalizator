namespace CurrencyRateBattle.WebAPI.Abstractions;

public interface IJsonConverter
{
    Task<List<T>> ConvertAsync<T>(Stream stream);
}
