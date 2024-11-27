using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Controllers.Models.Responses;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class DashboardController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        private readonly IMapper _mapper;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IOrderService orderService,
            ICustomerService customerService,
            IMapper mapper,
            ILogger<DashboardController> logger)
        {
            _orderService = orderService;
            _customerService = customerService;

            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<TotalResponse>> Totals()
        {
            var response = new TotalResponse();

            response.Revenue = await _orderService.RevenueTotals();
            response.Categories = await _orderService.CategoryRatios();
            response.CategoryRevenue = await _orderService.CategoryRevenue();
            response.PendingShipments = await _orderService.PendingShipments();
            response.CategoryHeatmap = await _orderService.CategoryHeatmap();

            return response;
        }
    }
}
