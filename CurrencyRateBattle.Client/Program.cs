using CurrencyRateBattle.Client;
using CurrencyRateBattle.Client.Dtos;

while (true)
{
    var user = new UserDto();
    var userInfo = new UserInfo();

    user = await Authorization();
    while (true)
    {
        ActionHelper.MainMenu();
        Console.WriteLine("Choose action:");
        var ch = Console.ReadLine();
        switch (ch)
        {
            case "1":
                var rooms = await userInfo.GetAllRooms();
                Console.Clear();

                if (rooms == null)
                {
                    ActionHelper.WrongData("There are no rooms at the moment");
                    break;
                }

                var activeRooms = rooms.Where(x => x.EndDateTime > DateTimeOffset.Now).ToList();
                ActionHelper.PrintList<RoomDto>(activeRooms);

                break;

            case "2":
                Console.WriteLine("Write your number:");
                var roomNum = Console.ReadLine();
                var roomsList = await userInfo.GetAllRooms();

                if (int.TryParse(roomNum, out var number) && roomsList != null && number > 0 && number <= roomsList.Count)
                {
                    string? backAction = "";

                    while (backAction != "3")
                    {
                        Console.Clear();
                        ActionHelper.PrintList(new List<RoomDto?> { roomsList[number - 1] });
                        ActionHelper.BetMenu();

                        Console.WriteLine("Choose action:");
                        var action = Console.ReadLine();
                        ActionHelper.Line();

                        await Bet(roomsList[number - 1], user, action);
                        backAction = action;
                    }
                }
                else
                {
                    ActionHelper.WrongData("There are no such room number at the moment");
                }

                break;

            case "3":
                var balance = await userInfo.GetBalance(user!.Id);
                Console.Clear();

                if (balance == -1)
                {
                    ActionHelper.WrongData("Can`t find your balance");
                    break;
                }

                ActionHelper.Line();
                Console.WriteLine($"Your balance: {balance} UAH");
                ActionHelper.Line();
                break;

            case "4":
                var history = await userInfo.GetHistory(user!.Id);
                Console.Clear();

                if (history == null || history.Count == 0)
                {
                    ActionHelper.WrongData("There are no history at the moment");
                    break;
                }

                history.Reverse();

                foreach (var item in history)
                {
                    var color = ConsoleColor.White;

                    if (item?.Win <= 0)
                        color = ConsoleColor.Red;

                    if (item?.Win > 0)
                        color = ConsoleColor.Green;

                    Console.ForegroundColor = color;
                    Console.WriteLine(item);

                    Console.ForegroundColor = ConsoleColor.White;
                    ActionHelper.Line();
                }

                break;


        }
    }
}

async Task<UserDto?> Authorization()
{
    UserDto? user = null;

    while (user is null || user.Id == default)
    {
        ActionHelper.ActionToSignIn();
        Console.WriteLine("Choose action:");
        var ch = Console.ReadLine();
        ActionHelper.Line();

        switch (ch)
        {
            case "1":
                user = await Authentication.LogInAsync();

                if (user is null)
                {
                    ActionHelper.WrongData("Wrong Login or Password");
                    ActionHelper.Line();
                }
                break;

            case "2":
                user = await Authentication.RegisterAsync();

                if (user is null)
                {
                    ActionHelper.WrongData("This Emais or Username is already exist!");
                    ActionHelper.Line();
                }
                break;
        }
    }

    Console.Clear();
    ActionHelper.SuccessfulAction("autorized");
    ActionHelper.Line();

    return user;
}

async Task Bet(RoomDto room, UserDto user, string? action)
{
    var menu = new BetMenu();

    switch (action)
    {
        case "1":
            var response = menu.MakeABet(room, user).Result;

            if (response.IsSuccess)
            {
                ActionHelper.SuccessfulAction(response.Response + "\n Now your balance is " + response.CurrentBalance);
            }
            else
            {
                ActionHelper.WrongData(response.Response);
            }

            Console.WriteLine("Press Enter to go back");
            Console.ReadLine();
            break;

        case "2":
            await menu.ShowPlayers(new PlayerInfoDto() { RoomId = room.Id });
            Console.WriteLine("Press Enter to go back");
            Console.ReadLine();
            break;

        default:
            break;
    }

    Console.Clear();
    return;
}
