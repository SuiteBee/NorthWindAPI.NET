using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface IOrderRepository
    {
        public Task<Order?> FindOrder(int id);
        public Task<List<OrderDetail>> FindDetail(int orderId);
        public Task<Product> FindProduct(int productId);
        public Task<Category> FindCategory(int categoryId);
        public Task<Shipper> FindCarrier(int shipperId);
    }
}
