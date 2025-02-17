using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Infrastructure.Exceptions.Base;
using NorthWindAPI.Infrastructure.Exceptions.Repository;

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
            try
            {
                return await _baseOrderRepo.ReturnEntityListAsync();
            }
            catch(EntityNotFoundException ex)
            {
                throw new OrderNotFoundException($"Orders {ex.Message}");
            }
        }

        public async Task<IEnumerable<OrderDetail>> AllDetails()
        {
            try
            {
                return await _baseDetailRepo.ReturnEntityListAsync();
            }
            catch (EntityNotFoundException ex)
            {
                throw new OrderDetailNotFoundException($"Details {ex.Message}");
            }
        }

        public async Task<IEnumerable<Shipper>> AllCarriers()
        {
            var carriers = await _context.Shipper.ToListAsync();
            if(carriers == null || carriers.Count == 0)
            {
                throw new CarrierNotFoundException($"Carriers not found");
            }
            return carriers;
        }

        #endregion

        #region " GET "

        public async Task<Order?> FindOrder(int id)
        {
            try
            {
                return await _baseOrderRepo.FindEntityAsync(id);
            }
            catch (EntityNotFoundException ex)
            {
                throw new OrderNotFoundException($"Order {ex.Message}");
            }
        }

        public async Task<List<OrderDetail>> FindDetail(int orderId)
        {
            var details = QueryOrderDetails().Where(x => x.OrderId == orderId);
            if(details == null || !details.Any())
            {
                throw new OrderDetailNotFoundException($"Details for order {orderId} not found");
            }
            return await details.ToListAsync();
        }


        public async Task<Shipper> FindCarrier(int shipperId)
        {
            var carriers = QueryShippers().Where(x => x.Id == shipperId);
            if(carriers == null || !carriers.Any())
            {
                throw new CarrierNotFoundException($"Carrier {shipperId} not found");
            }
            return await carriers.FirstAsync();
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

        public IEnumerable<OrderDetail> InsertDetails(IEnumerable<OrderDetail> details)
        {
            try
            {
                return _baseDetailRepo.AddMultipleEntities(details);
            }
            catch (EntityNotCreatedException ex)
            {
                throw new OrderDetailNotCreatedException($"Details {ex.Message}");
            }
            
        }

        public Order InsertOrder(Order order)
        {
            try
            {
                return _baseOrderRepo.AddEntity(order);
            }
            catch (EntityNotCreatedException ex)
            {
                throw new OrderNotCreatedException($"Order {ex.Message}");
            }
        }

        #endregion

        #region " PUT "

        public Order UpdateOrder(int orderId, Order order)
        {
            try
            {
                return _baseOrderRepo.UpdateEntity(orderId, order);
            }
            catch (EntityNotUpdatedException ex)
            {
                throw new OrderNotUpdatedException($"Order {ex.Message}");
            }
        }

        #endregion

        #region " DELETE "

        public async Task DeleteOrder(int id)
        {
            try
            {
                List<OrderDetail> detailsToRemove = await FindDetail(id);
                foreach (OrderDetail detail in detailsToRemove)
                {
                    await _baseDetailRepo.RemoveEntityAsync(detail.Id);
                }
                await _baseOrderRepo.RemoveEntityAsync(id);
            }
            catch(Exception ex)
            {
                throw new OrderNotRemovedException($"Order not removed : {ex.Message}");
            }
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
