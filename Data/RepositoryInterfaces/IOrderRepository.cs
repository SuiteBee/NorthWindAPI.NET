using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> AllOrders();
        public Task<IEnumerable<OrderDetail>> AllDetails();
        public Task<IEnumerable<Shipper>> AllCarriers();

        public Task<Order?> FindOrder(int id);
        public Task<List<OrderDetail>> FindDetail(int orderId);
        public Task<Shipper> FindCarrier(int shipperId);

        public Task<IEnumerable<OrderDetail>> InsertDetails(IEnumerable<OrderDetail> details);
        public Task<Order> InsertOrder(Order order);

        public Task<Order> UpdateOrder(int orderId, Order order);

        public Task<bool> DeleteOrder(int id);
    }
}
