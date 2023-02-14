using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using CurrencyRateBattle.WebAPI.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Entities;
using CurrencyRateBattle.WebAPI.Dtos;
using NLog;

namespace CurrencyRateBattle.WebAPI.Services;

public class AuthenticationService : IAuthenticationService
{

    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    public AuthenticationService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }


    public UserDto? Login(UserLoginDto userLogin)
    {
        _logger.Info("Getting user by email or username: {emailOrUsername}", userLogin.EmailOrUsername);
        var user = _userRepository.GetByEmailOrUsername(userLogin?.EmailOrUsername);

        if (user == null || user.Password != EncryptPassword(userLogin?.Password))
        {
            _logger.Warn("The provided password: {Password} doesn`t match the user`s password'", userLogin.Password);
            return null;
        }

        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }

    public async Task<UserDto?> RegisterAsync(UserRegistrationDto userRegister)
    {

        const int startBalance = 2000;
        _logger.Info("Check if there is a user with such a login or email");
        var isUserExist = _userRepository.IsUserExist(userRegister.Email, userRegister.Login);

        if (isUserExist)
        {
            _logger.Warn("Such email or username is already exist");
            return null;
        }

        var user = _mapper.Map<User>(userRegister);

        user.Password = EncryptPassword(userRegister.Password);
        user.Balance = startBalance;

        _logger.Info("Write user to database");
        user.Id = await _userRepository.Add(user);
        await _userRepository.SaveChangesAsync();
        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }

    private string EncryptPassword(string password)
    {
        using var sha256 = SHA256.Create();
        return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "");
    }
}
