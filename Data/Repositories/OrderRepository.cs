using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly IBaseRepository<Order> _baseOrderRepo;
        private readonly IBaseAltRepository<OrderDetail> _baseDetailRepo;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
            _baseOrderRepo = new BaseRepository<Order>(_context);
            _baseDetailRepo = new BaseAltRepository<OrderDetail>(_context);
        }

        #region " GET ALL "

        public async Task<IEnumerable<Order>> AllOrders()
        {
            return await _baseOrderRepo.ReturnEntityListAsync();
        }

        public async Task<IEnumerable<OrderDetail>> AllDetails()
        {
            return await _baseDetailRepo.ReturnEntityListAsync();
        }

        public async Task<IEnumerable<Shipper>> AllCarriers()
        {
            return await _context.Shipper.ToListAsync();
        }

        #endregion

        #region " GET "

        public async Task<Order?> FindOrder(int id)
        {
            return await _baseOrderRepo.FindEntityAsync(id);
        }

        public async Task<List<OrderDetail>> FindDetail(int orderId)
        {
            return await QueryOrderDetails().Where(x => x.OrderId == orderId).ToListAsync();
        }


        public async Task<Shipper> FindCarrier(int shipperId)
        {
            return await QueryShippers().Where(x => x.Id == shipperId).FirstAsync();
        }

        private IQueryable<OrderDetail> QueryOrderDetails()
        {
            return _context.OrderDetail.AsQueryable();
        }

        private IQueryable<Shipper> QueryShippers()
        {
            return _context.Shipper.AsQueryable();
        }

        #endregion

        #region " POST "

        public async Task<IEnumerable<OrderDetail>> InsertDetails(IEnumerable<OrderDetail> details)
        {
            return await _baseDetailRepo.AddMultipleEntitiesAsync(details);
        }

        public async Task<Order> InsertOrder(Order order)
        {
            return await _baseOrderRepo.AddEntityAsync(order);
        }

        #endregion

        #region " PUT "

        public async Task<Order> UpdateOrder(int orderId, Order order)
        {
            return await _baseOrderRepo.UpdateEntityAsync(orderId, order);
        }

        #endregion

        #region " DELETE "

        public async Task DeleteOrder(int id)
        {
            List<OrderDetail> detailsToRemove = await FindDetail(id);
            foreach (OrderDetail detail in detailsToRemove)
            {
                await _baseDetailRepo.RemoveEntityAsync(detail.Id);
            }
            await _baseOrderRepo.RemoveEntityAsync(id);
        }

        #endregion

        public IDbContextTransaction BeginTransaction()
        {
            _context.Database.OpenConnection();
            return _context.Database.BeginTransaction();
        }

        public void CommitTransaction(IDbContextTransaction transaction)
        {
            try
            {
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _context.Database.CloseConnection();
            }

            _context.Database.CloseConnection();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
