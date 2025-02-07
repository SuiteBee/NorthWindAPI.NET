using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Interfaces
{
    public interface IProductService
    {
        public Task<ProductDto> FindProduct(int id);
        public Task<IEnumerable<ProductDto>> ListProducts();
        public Task<ProductDto> PriceUpdate(int id, PriceRequest price);
        public Task<ProductDto> AddStock(int id, StockRequest stock);
        public Task RemoveStock(IEnumerable<OrderDetailRequest> orderDetails);
        public Task ReplaceStock(IEnumerable<OrderItemDto> orderItems);
        public Task<ProductDto> Update(int id, ProductRequest prod);

    }
}
