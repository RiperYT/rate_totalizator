using CurrencyRateBattle.WebAPI.Dtos;

namespace CurrencyRateBattle.WebAPI.Abstractions;

public interface IAuthenticationService
{
    Task<UserDto?> RegisterAsync(UserRegistrationDto userRegister);

    UserDto? Login(UserLoginDto userLogin);
}
