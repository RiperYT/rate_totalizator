namespace CurrencyRateBattle.WebAPI.Data.Abstractions;

public interface IDbRepository<T> where T : class, IEntity
{

    T? GetById(long id);
    List<T> GetAll();

    Task<long> Add(T entity);
    Task AddRange(IEnumerable<T> entities);

    Task SetActiveFalse(long id);

    Task Update(T entity);
    Task UpdateRange(IEnumerable<T> entities);

    Task Delete(int id);

    Task<int> SaveChangesAsync();
}
