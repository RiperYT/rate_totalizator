using CurrencyRateBattle.WebAPI.Abstractions;
using CurrencyRateBattle.WebAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattle.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authentificationService;

    public AuthenticationController(IAuthenticationService authentificationService)
    {
        _authentificationService = authentificationService;
    }

    [HttpPost("register")]
    public Task<UserDto?> Register(UserRegistrationDto user)
    {
        return _authentificationService.RegisterAsync(user);
    }

    [HttpPost("login")]
    public UserDto? Login(UserLoginDto user)
    {
        return _authentificationService.Login(user);
    }

}
