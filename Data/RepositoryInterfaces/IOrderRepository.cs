﻿using Microsoft.EntityFrameworkCore.Storage;
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

        public IEnumerable<OrderDetail> InsertDetails(IEnumerable<OrderDetail> details);
        public Order InsertOrder(Order order);

        public Order UpdateOrder(int orderId, Order order);

        public Task DeleteOrder(int id);

        public IDbContextTransaction BeginTransaction();
        public void CommitTransaction(IDbContextTransaction transaction);
        public Task Save();
    }
}
