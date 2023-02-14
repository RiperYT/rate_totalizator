namespace CurrencyRateBattle.Client.Dtos;

public class RoomDto
{
    public int Id { get; set; }

    public CurrencyDto? Currency { get; set; }

    public DateTimeOffset EndDateTime { get; set; }

    public DateTimeOffset CloseAccessTime { get; set; }

    public TypeGame TypeGame { get; set; }

    public int CountParticipants { get; set; }

    public decimal Rate { get; set; }

    public decimal Sum { get; set; }

    public override string ToString()
    {
        return $"Currency: {Currency?.CurrencyName}\n" +
            $"Time course: {EndDateTime.DateTime}\n" +
            $"Close access room: {CloseAccessTime.DateTime}\n" +
            $"Count participants: {CountParticipants}\n" +
            $"Rate: {Rate}\n" +
            $"Total amount: {Sum}\n" +
            $"Type game: {TypeGame}";
    }
}
