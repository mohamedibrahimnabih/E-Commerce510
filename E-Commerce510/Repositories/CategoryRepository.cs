using E_Commerce510.Data;
using E_Commerce510.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Commerce510.Repositories
{
    public class CategoryRepository
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        // CRUD
        public void Create(Category category)
        {
            _dbContext.Categories.Add(category);
        }
        public void CreateAll(List<Category> categories)
        {
            _dbContext.Categories.AddRange(categories);
        }
        public void Edit(Category category)
        {
            _dbContext.Categories.Update(category);
        }
        public void Delete(Category category)
        {
            _dbContext.Categories.Remove(category);
        }
        public void DeleteAll(List<Category> categories)
        {
            _dbContext.Categories.RemoveRange(categories);
        }
        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public IQueryable<Category> Get(Expression<Func<Category, bool>>? filter = null, Expression<Func<Category, object>>[]? includes = null, bool tracked = true)
        {
            IQueryable<Category> query = _dbContext.Categories;

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

        public Category? GetOne(Expression<Func<Category, bool>>? filter = null, Expression<Func<Category, object>>[]? includes = null, bool tracked = true)
        {
            return Get(filter, includes, tracked).FirstOrDefault();
        }
    }
}
