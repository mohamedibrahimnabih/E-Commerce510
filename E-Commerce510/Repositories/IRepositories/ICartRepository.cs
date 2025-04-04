using E_Commerce510.Models;

namespace E_Commerce510.Repositories.IRepositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        public void DeleteAll(List<Cart> entities);
    }
}
