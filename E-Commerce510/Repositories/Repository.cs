using E_Commerce510.Data;
using E_Commerce510.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using E_Commerce510.Repositories.IRepositories;

namespace E_Commerce510.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public DbSet<T> dbSet;
        private readonly ApplicationDbContext _dbContext;
        public Repository(ApplicationDbContext dbContext)
        {
            dbSet = _dbContext.Set<T>();
            this._dbContext = dbContext;
        }

        // CRUD
        public void Create(T entity)
        {
            dbSet.Add(entity);
        }
        public void CreateAll(List<T> entities)
        {
            dbSet.AddRange(entities);
        }
        public void Edit(T entity)
        {
            dbSet.Update(entity);
        }
        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }
        public void DeleteAll(List<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }

            if (tracked)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public T? GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            return Get(filter, includes, tracked).FirstOrDefault();
        }
    }
}
