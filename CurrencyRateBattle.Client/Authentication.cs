using CurrencyRateBattle.Client.Dtos;
using System.Text;
using System.Text.Json;

namespace CurrencyRateBattle.Client;

public class Authentication
{
    private const string BaseApiUrl = "http://localhost:5124";

    public static async Task<UserDto?> RegisterAsync()
    {
        var userRegister = EnterRegistrationData();
        var json = JsonSerializer.Serialize(userRegister);

        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        var httpClient = new HttpClient();

        var responseMessage = await httpClient.PostAsync($"{BaseApiUrl}/api/authentication/register", stringContent);
        var response = await responseMessage.Content.ReadAsStringAsync();

        var user = ConvertingHelper.Deserialize<UserDto?>(response);

        return user;
    }

    public static async Task<UserDto?> LogInAsync()
    {
        var userLogin = EnterLoginData();
        var json = JsonSerializer.Serialize(userLogin);

        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        var httpClient = new HttpClient();

        var responseMessage = await httpClient.PostAsync($"{BaseApiUrl}/api/authentication/login", stringContent);
        var response = await responseMessage.Content.ReadAsStringAsync();

        var user = ConvertingHelper.Deserialize<UserDto?>(response);

        return user;
    }

    private static UserLoginDto EnterLoginData()
    {
        var userLogin = new UserLoginDto();

        Console.WriteLine("Enter email or username:");
        userLogin.EmailOrUsername = Console.ReadLine();

        Console.WriteLine("Enter password:");
        userLogin.Password = EnterPassword();

        return userLogin;
    }

    private static UserRegistrationDto? EnterRegistrationData()
    {
        var userRegister = new UserRegistrationDto();

        Console.WriteLine("Enter email:");
        userRegister.Email = Console.ReadLine();

        Console.WriteLine("Enter username:");
        userRegister.Login = Console.ReadLine();

        Console.WriteLine("Enter password:");
        userRegister.Password = EnterPassword();

        if (userRegister.Email == string.Empty || userRegister.Login == string.Empty || userRegister.Password == string.Empty)
            return null;

        return userRegister;
    }

    private static string? EnterPassword()
    {
        var isIncorrectPassword = true;
        var attempts = 0;
        var password = string.Empty;

        while (isIncorrectPassword)
        {
            if (attempts >= 3)
            {
                return string.Empty;
            }

            password = Console.ReadLine();
            isIncorrectPassword = IsCorrectPassword(password);
            if (isIncorrectPassword)
            {
                attempts++;
                ActionHelper.WrongData("Password is too simple!\n" +
                    "Password must be at least 6 characters long and not simple");

                Console.WriteLine("Try again:");
            }
        }

        return password;
    }

    private static bool IsCorrectPassword(string? password)
    {
        return password is null || password is "" || password.Length < 6
                || password.All(x => x == password[0]) || password is "123456";
    }
}
