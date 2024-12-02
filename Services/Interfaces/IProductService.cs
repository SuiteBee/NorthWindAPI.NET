using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Interfaces
{
    public interface IProductService
    {
        public Task<ProductDto> FindProduct(int id);
        public Task<IEnumerable<ProductDto>> ListProducts();
        public Task<ProductDto> PriceUpdate(int id, ProductDto prod);
        public Task<ProductDto> AddStock(int id, ProductDto prod);
        public Task<ProductDto> Update(int id, ProductDto prod);

    }
}
