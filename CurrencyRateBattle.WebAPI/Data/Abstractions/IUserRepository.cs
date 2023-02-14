using CurrencyRateBattle.WebAPI.Data.Entities;

namespace CurrencyRateBattle.WebAPI.Data.Abstractions;

public interface IUserRepository : IDbRepository<User>
{

    Task UpdateUserBalanceAsync(long id, decimal cash);
    bool IsUserExist(string? email, string? username);
    User GetByEmailOrUsername(string? emailOrUsename);
    string GetUsernameByUserID(long userId);

}
