using E_Commerce510.Models;

namespace E_Commerce510.Repositories.IRepositories
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        public void CreateAll(List<OrderItem> entities);
    }
}
