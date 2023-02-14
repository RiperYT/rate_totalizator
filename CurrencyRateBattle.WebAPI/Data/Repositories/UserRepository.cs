using CurrencyRateBattle.WebAPI.Data.Abstractions;
using CurrencyRateBattle.WebAPI.Data.Entities;

namespace CurrencyRateBattle.WebAPI.Data.Repositories;

public class UserRepository : DbRepository<User>, IUserRepository
{

    public UserRepository(DataContext context) : base(context) { }

    public async Task UpdateUserBalanceAsync(long id, decimal cash)
    {
        var user = GetById(id);
        if (user != null)
        {
            user.Balance += cash;
            await Update(user);
        }
    }

    public User? GetByEmailOrUsername(string? emailOrUsename)
    {
        return GetAll().FirstOrDefault(x => x.Email == emailOrUsename || x.Login == emailOrUsename);
    }

    public bool IsUserExist(string? email, string? username)
    {
        return GetAll().FirstOrDefault(x => x.Email == email || x.Login == username) is not null;
    }

    public string GetUsernameByUserID(long userId)
    {
        return GetAll().First(x => x.Id == userId).Login;
    }
}
