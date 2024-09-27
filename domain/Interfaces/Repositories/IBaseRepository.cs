using System.Linq.Expressions;
using domain.Entities;

namespace domain.Interfaces.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task Add(T entity);
    Task Remove(T entity);
    Task Update(T entity);
    Task<T> GetById(int id,  Expression<Func<T, object>>[]? includeProperties);
    Task<IEnumerable<T>> GetAll();
}
