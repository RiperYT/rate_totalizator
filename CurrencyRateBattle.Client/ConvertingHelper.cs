using System.Text.Json;

namespace CurrencyRateBattle.Client;

public static class ConvertingHelper
{
    public static T? Deserialize<T>(string? json)
    {
        if (json == null || json == string.Empty)
            return default;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<T?>(json, options);
    }


}
