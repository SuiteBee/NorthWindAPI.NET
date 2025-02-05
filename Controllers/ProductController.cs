using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Controllers.Models.Responses;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Services;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Services.ResponseDto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NorthWindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, IMapper mapper, ILogger<ProductController> logger)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get list of all available products
        /// </summary>
        [ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> All()
        {
            var products = await _productService.ListProducts();
            var productResponse = _mapper.Map<List<ProductResponse>>(products);

            if (productResponse == null || productResponse.Count == 0)
            {
                return NotFound();
            }

            return productResponse;
        }

        /// <summary>
        /// Get single product record by ID
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> Find(int id)
        {
            var product = await _productService.FindProduct(id);
            var productResponse = _mapper.Map<ProductResponse>(product);

            if (productResponse == null)
            {
                return NotFound();
            }

            return productResponse;
        }

        /// <summary>
        /// Update product record cost + Return product record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ForbidResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponse>> Price(int id, PriceRequest price)
        {
            var product = new ProductDto();

            try
            {
                product = await _productService.PriceUpdate(id, price);
            }
            catch
            {
                return Forbid();
            }

            var response = _mapper.Map<ProductResponse>(product);
            return response;
        }

        /// <summary>
        /// Update product record stock quantity + Return product record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stock"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ForbidResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponse>> Stock(int id, StockRequest stock)
        {
            var product = new ProductDto();

            try
            {
                product = await _productService.AddStock(id, stock);
            }
            catch
            {
                return Forbid();
            }

            var response = _mapper.Map<ProductResponse>(product);
            return response;
        }

        /// <summary>
        /// Update product record + Return product record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="prod"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ForbidResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponse>> Update(int id, ProductRequest prod)
        {
            var product = new ProductDto();

            try
            {
                if (id != prod.ProductId)
                {
                    return BadRequest();
                }

                product = await _productService.Update(id, prod);
            }
            catch
            {
                return Forbid();
            }

            var response = _mapper.Map<ProductResponse>(product);
            return response;
        }
    }
}
