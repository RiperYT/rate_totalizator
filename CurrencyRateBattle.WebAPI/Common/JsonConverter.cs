using System.Text.Json;
using CurrencyRateBattle.WebAPI.Abstractions;

namespace CurrencyRateBattle.WebAPI.Common;

public class JsonConverter : IJsonConverter
{
    public async Task<List<T>> ConvertAsync<T>(Stream stream)
    {
        return await JsonSerializer.DeserializeAsync<List<T>>(stream) ?? new List<T>();
    }
}
