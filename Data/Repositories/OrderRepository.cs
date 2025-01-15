using Microsoft.EntityFrameworkCore;
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

        public async Task<OrderDetail> InsertDetail(OrderDetail detail)
        {
            return await _baseDetailRepo.AddEntityAsync(detail);
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

        /// <summary>
        /// Delete all associated detail records and delete parent record. Changes are saved when parent is removed.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteOrder(int id)
        {
            List<OrderDetail> detailsToRemove = await FindDetail(id);
            foreach (OrderDetail detail in detailsToRemove)
            {
                await _baseDetailRepo.RemoveDependentEntityAsync(detail.Id);
            }
            return await _baseOrderRepo.RemoveEntityAsync(id);
        }

        #endregion



        //public async Task<List<object>> FindFullOrder(int id)
        //{
        //    var sqlRaw =
        //        "SELECT * " +
        //        "FROM " +
        //        "FROM [Order] o " +
        //        "JOIN [Employee] e ON e.Id = o.EmployeeId " +
        //        "JOIN [OrderDetail] od ON o.Id = od.OrderId " +
        //        "JOIN [Product] p ON p.Id = od.ProductId " +
        //        $"WHERE o.Id = {id}";

        //    var query = FormattableStringFactory.Create(sqlRaw);
        //    return await _context.Database.SqlQuery<object>(query).ToListAsync();
        //}
    }
}
