using E_Commerce510.Data;
using E_Commerce510.Models;
using E_Commerce510.Repositories.IRepositories;

namespace E_Commerce510.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
