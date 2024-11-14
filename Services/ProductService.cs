using AutoMapper;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, IMapper mapper, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProductDto> FindProduct(int id)
        {
            Product product = await _productRepository.FindProduct(id);
            Category cat = await _productRepository.FindCategory(product.CategoryId);

            ProductDto prodDto = _mapper.Map<ProductDto>(product);
            _mapper.Map(cat, prodDto);

            Supplier suppliedBy = await _productRepository.FindSupplier(product.SupplierId);
            _mapper.Map(suppliedBy, prodDto.Supplier);
            _mapper.Map(suppliedBy, prodDto.Supplier.Address);
            _mapper.Map(suppliedBy, prodDto.Supplier.ContactInfo);

            return prodDto;
        }

        public async Task<IEnumerable<ProductDto>> ListProducts()
        {
            var products = await _productRepository.AllProducts();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}
