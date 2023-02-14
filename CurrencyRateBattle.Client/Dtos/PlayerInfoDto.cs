namespace CurrencyRateBattle.Client.Dtos;

public class PlayerInfoDto
{
    public int RoomId { get; set; }

    public List<string>? Usernames { get; set; }

    public List<decimal>? Bets { get; set; }

    public override string ToString()
    {
        var result = "";

        if (Usernames is not null && Bets is not null)
        {
            for (var i = 0; i < Usernames.Count; i++)
            {
                result += Usernames[i] + " ";
                result += Bets[i] + "\n";
            }
        }

        return result;
    }
}
