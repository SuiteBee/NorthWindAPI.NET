using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Controllers.Models.Responses;
using NorthWindAPI.Views.Interfaces;

namespace NorthWindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardView _dashboardView;

        private readonly IMapper _mapper;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IDashboardView dashboardView,
            IMapper mapper,
            ILogger<DashboardController> logger)
        {
            _dashboardView = dashboardView;

            _mapper = mapper;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ChartsResponse>> Charts()
        {
            var response = new ChartsResponse();

            response.Totals = await _dashboardView.GetTotals();
            response.Revenue = await _dashboardView.RevenueTotals();
            response.Categories = await _dashboardView.CategoryRatios();
            response.CategoryRevenue = await _dashboardView.CategoryRevenue();
            response.PendingShipments = await _dashboardView.PendingShipments();
            response.CategoryHeatmap = await _dashboardView.CategoryHeatmap();

            return response;
        }
    }
}
