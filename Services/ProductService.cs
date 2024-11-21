﻿using AutoMapper;
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
        public async Task<IEnumerable<ProductDto>> ListProducts()
        {
            var productList = await _productRepository.AllProducts();
            var categoryList = await _productRepository.AllCategories();
            var supplierList = await _productRepository.AllSuppliers();

            var productDto = _mapper.Map<IEnumerable<ProductDto>>(productList);

            foreach (ProductDto dto in productDto)
            {
                Product prod = productList.First(x => x.Id == dto.ProductId);
                Category cat = categoryList.First(x => x.Id == prod.CategoryId);
                Supplier sup = supplierList.First(x => x.Id == prod.SupplierId);

                _mapper.Map(cat, dto);
                _mapper.Map(sup, dto.SuppliedBy);
                _mapper.Map(sup, dto.SuppliedBy.Address);
                _mapper.Map(sup, dto.SuppliedBy.ContactInfo);

                dto.InStock = dto.StockAmt > 0;
            }

            return productDto;
        }

        public async Task<ProductDto> FindProduct(int id)
        {
            Product product = await _productRepository.FindProduct(id);
            Category cat = await _productRepository.FindCategory(product.CategoryId);
            Supplier supplier = await _productRepository.FindSupplier(product.SupplierId);

            ProductDto prodDto = _mapper.Map<ProductDto>(product);
            _mapper.Map(cat, prodDto);
            _mapper.Map(supplier, prodDto.SuppliedBy);
            _mapper.Map(supplier, prodDto.SuppliedBy.Address);
            _mapper.Map(supplier, prodDto.SuppliedBy.ContactInfo);

            prodDto.InStock = prodDto.StockAmt > 0;

            return prodDto;
        }
    }
}
