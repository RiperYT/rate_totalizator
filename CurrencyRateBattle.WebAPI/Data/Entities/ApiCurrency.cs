using System.Text.Json.Serialization;
using CurrencyRateBattle.WebAPI.Common;

namespace CurrencyRateBattle.WebAPI.Data.Entities;

public class ApiCurrency
{

    [JsonPropertyName("currencyCodeA")]
    public int CurrencyA { get; set; }

    [JsonPropertyName("currencyCodeB")]
    public int CurrencyB { get; set; }

    [JsonPropertyName("rateSell")]
    public decimal RateSell { get; set; }

    [JsonPropertyName("rateBuy")]
    public decimal RateBuy { get; set; }

    [JsonPropertyName("rateCross")]
    public decimal Rate { get; set; }

    [JsonPropertyName("date")]
    [JsonConverter(typeof(ConvertDateToNormal))]
    public DateTime DateUpdate { get; set; }

    public Currency ConvertToDbView()
    {
        if (CurrencyB != 980)
        {
            return new Currency();
        }

        if (Rate == default)
        {
            Rate = (RateBuy + RateSell) / 2m;
        }

        return new Currency()
        {
            UpdateDateTime = DateUpdate,
            RateActual = Rate,
            CurrencyName = ConvertCurrencyName(CurrencyA)
        };
    }

    private string ConvertCurrencyName(int cc)
    {
        switch (cc)
        {
            case 840:
                return "USD";
            case 978:
                return "EUR";
            case 985:
                return "PLN";
            case 826:
                return "GBP";
            case 756:
                return "CHF";
            default:
                return cc.ToString();
        }
    }
}
