using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace E_Commerce510.Repositories.IRepositories
{
    public interface IRepository<T>
    {
        // CRUD
        public void Create(T entity);
        public void Edit(T entity);
        public void Delete(T entity);
        public void Commit();

        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);

        public T? GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);
    }
}
