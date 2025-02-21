using E_Commerce510.Data;
using E_Commerce510.Models;
using E_Commerce510.Repositories.IRepositories;

namespace E_Commerce510.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
