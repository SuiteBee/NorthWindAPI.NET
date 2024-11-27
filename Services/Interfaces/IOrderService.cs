using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDto>> ListOrders();
        public Task<OrderDto> FindOrder(int orderId);
        public Task<OrderDto> ProcessNewOrder(NewOrderRequest newOrder);
        public Task<OrderDto> MarkAsShipped(int orderId, string? shippedDate = null);
        public Task<IEnumerable<OrderDto>> MarkAsShipped(ShipRequest orders);
        public Task<bool> RemoveOrder(int orderId);

        public Task<IEnumerable<RevenueDto>> RevenueTotals();
        public Task<IEnumerable<CategoryRatiosDto>> CategoryRatios();
        public Task<IEnumerable<CategoryRevenueDto>> CategoryRevenue();
        public Task<int> PendingShipments();
        public Task<IEnumerable<CategoryHeatmapDto>> CategoryHeatmap();
    }
}

