namespace CurrencyRateBattle.Client.Dtos;

public class RoomUserDto
{
    public int Id { get; set; }

    public RoomDto? Room { get; set; }

    public decimal Bet { get; set; }

    public decimal? Win { get; set; }

    public override string ToString()
    {
        return $"Currency: {Room.Currency.CurrencyName}\n" +
            $"Time course: {Room.EndDateTime.DateTime}\n" +
            $"Type game: {Room.TypeGame}\n" +
            $"Rate: {Room.Rate}\n" +
            $"Win: {Win}";
    }

}
