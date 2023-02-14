using Microsoft.EntityFrameworkCore;

using CurrencyRateBattle.WebAPI.Data.Abstractions;

namespace CurrencyRateBattle.WebAPI.Data.Repositories;

public class DbRepository<T> : IDbRepository<T> where T : class, IEntity
{

    private readonly DataContext _context;

    public DbRepository(DataContext context)
    {
        _context = context;
    }

    public T? GetById(long id)
    {
        return _context.Set<T>().Where(t => t.Id == id).FirstOrDefault();
    }
    public List<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public async Task<long> Add(T entity)
    {
        var entit = await _context.Set<T>().AddAsync(entity);

        //await SaveChangesAsync();

        return entit.Entity.Id;
    }
    public async Task AddRange(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);

        //await SaveChangesAsync();
    }

    public async Task SetActiveFalse(long id)
    {
        var activeEntity = await _context.Set<T>().Where(t => t.Id == id).FirstOrDefaultAsync();
        if (activeEntity != null)
        {
            activeEntity.IsActive = false;
            _ = await Task.Run(() => _context.Update(activeEntity));

            //await SaveChangesAsync();
        }
    }

    public async Task Update(T entity)
    {
        _ = await Task.Run(() => _context.Set<T>().Update(entity));

        //await SaveChangesAsync();
    }

    public async Task UpdateRange(IEnumerable<T> entities)
    {
        await Task.Run(() => _context.Set<T>().UpdateRange(entities));

        //await SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _context.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id);
        if (entity != null)
            _ = _context.Set<T>().Remove(entity);


        //await SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();

        //if (result <= 0)
        //{
        //    throw new InvalidOperationException();
        //}
    }
}
