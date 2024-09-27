using System.Linq.Expressions;
using domain.Entities;
using domain.Interfaces.Repositories;
using infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace infra.Data.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly NpSqlContext _npSqlContext;

    public BaseRepository(NpSqlContext npSqlContext)
    {
        _npSqlContext = npSqlContext;
    }

    public async Task Add(T entity)
    {
        _npSqlContext.Set<T>().Add(entity);
        await _npSqlContext.SaveChangesAsync();
        _npSqlContext.Entry(entity).State = EntityState.Detached;
    }

    public async Task Remove(T entity)
    {
        _npSqlContext.Set<T>().Remove(entity);
        await _npSqlContext.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        _npSqlContext.Entry(entity).State = EntityState.Modified;
        await _npSqlContext.SaveChangesAsync();
    }

    public async Task<T> GetById(int id, Expression<Func<T, object>>[]? includeProperties = null)
    {
        IQueryable<T> query = _npSqlContext.Set<T>().AsNoTracking();

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        return await query.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _npSqlContext.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> FindBy(Expression<Func<T, bool>> predicate,  Expression<Func<T, object>>[]? includeProperties = null, Expression<Func<T, object>>[]? thenIncludeProperties = null)
    {
        IQueryable<T> query = _npSqlContext.Set<T>().AsNoTracking();

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        return await query.AsNoTracking().Where(predicate).ToListAsync();
    }
}
