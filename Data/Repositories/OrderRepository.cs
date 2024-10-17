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

        public async Task<Order?> FindOrder(int id)
        {
            return await _baseOrderRepo.FindEntityAsync(id);
        }

        public async Task<OrderDetail?> FindDetail(int orderId)
        {
            var response = await _baseDetailRepo.ReturnEntityListAsync();
            return response.First(x => x.OrderId == orderId);
        }


    }
}
