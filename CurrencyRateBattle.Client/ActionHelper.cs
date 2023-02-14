namespace CurrencyRateBattle.Client;

public static class ActionHelper
{
    public static void ActionToSignIn()
    {
        Console.WriteLine("1. Log in");
        Console.WriteLine("2. Register");
    }

    public static void MainMenu()
    {
        Console.WriteLine("1. View rooms");
        Console.WriteLine("2. Choose room number");
        Console.WriteLine("3. View balance");
        Console.WriteLine("4. View history");
    }

    public static void BetMenu()
    {
        Console.WriteLine("1. Place a bet");
        Console.WriteLine("2. View players");
        Console.WriteLine("3. Go back");
    }

    public static void WrongData(string description)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{description}!");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
    }

    public static void SuccessfulAction(string description)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Successfully {description}!");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void PrintList<T>(List<T?>? list)
    {
        if (list == null)
            return;

        var number = 1;

        foreach (var item in list)
        {
            Console.WriteLine(number);
            Console.WriteLine(item);

            Console.ForegroundColor = ConsoleColor.Green;
            Line();
            Console.ForegroundColor = ConsoleColor.White;
            number++;
        }
    }

    public static void Line()
    {
        Console.WriteLine("______________________________________________");
    }
}
