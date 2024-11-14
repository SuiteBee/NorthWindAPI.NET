using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Interfaces
{
    public interface IProductService
    {
        public Task<ProductDto> FindProduct(int id);
        public Task<IEnumerable<ProductDto>> ListProducts();
        
    }
}
