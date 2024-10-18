using Microsoft.EntityFrameworkCore;
using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private IBaseRepository<Order> _baseOrderRepo;
        private IBaseAltRepository<OrderDetail> _baseDetailRepo;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
            _baseOrderRepo = new BaseRepository<Order>(_context);
            _baseDetailRepo = new BaseAltRepository<OrderDetail>(_context);
        }

        private IQueryable<OrderDetail> QueryOrderDetails()
        {
            return _context.OrderDetail.AsQueryable();
        }

        private IQueryable<Product> QueryProducts()
        {
            return _context.Product.AsQueryable();
        }

        private IQueryable<Category> QueryCategories()
        {
            return _context.Category.AsQueryable();
        }

        private IQueryable<Shipper> QueryShippers()
        {
            return _context.Shipper.AsQueryable();
        }

        public async Task<Order?> FindOrder(int id)
        {
            return await _baseOrderRepo.FindEntityAsync(id);
        }

        public async Task<List<OrderDetail>> FindDetail(int orderId)
        {
            return await QueryOrderDetails().Where(x => x.OrderId == orderId).ToListAsync();
        }

        public async Task<Product> FindProduct(int productId)
        {
            return await QueryProducts().Where(x => x.Id == productId).FirstAsync();
        }

        public async Task<Category> FindCategory(int categoryId)
        {
            return await QueryCategories().Where(x => x.Id == categoryId).FirstAsync();
        }

        public async Task<Shipper> FindCarrier(int shipperId)
        {
            return await QueryShippers().Where(x => x.Id == shipperId).FirstAsync();
        }

        
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
