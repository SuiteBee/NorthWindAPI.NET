using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> All()
        {
            var products = await _productService.ListProducts();
            if (products == null || !products.Any())
            {
                return NotFound();
            }

            return products.ToList();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Find(int id)
        {
            var product = await _productService.FindProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Price(int id, ProductDto prod)
        {
            return await _productService.PriceUpdate(id, prod);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Stock(int id, ProductDto prod)
        {
            return await _productService.AddStock(id, prod);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Update(int id, ProductDto prod)
        {
            return await _productService.Update(id, prod);
        }
    }
}
