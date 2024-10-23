using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> AllOrders();
        public Task<IEnumerable<OrderDetail>> AllDetails();
        public Task<IEnumerable<Product>> AllProducts();
        public Task<IEnumerable<Category>> AllCategories();
        public Task<IEnumerable<Shipper>> AllCarriers();

        public Task<Order?> FindOrder(int id);
        public Task<List<OrderDetail>> FindDetail(int orderId);
        public Task<Product> FindProduct(int productId);
        public Task<Category> FindCategory(int categoryId);
        public Task<Shipper> FindCarrier(int shipperId);

        public Task<OrderDetail> InsertDetail(OrderDetail detail);
        public Task<Order> InsertOrder(Order order);
    }
}
