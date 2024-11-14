using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Services;
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
    }
}
