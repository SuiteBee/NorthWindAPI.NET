using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Controllers.Models.Responses;
using NorthWindAPI.Views.Interfaces;

namespace NorthWindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class DashboardController(
        IDashboardView dashboardView,
        IMapper mapper,
        ILogger<DashboardController> logger
    ) : ControllerBase
    {
        private readonly IDashboardView _dashboardView = dashboardView;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<DashboardController> _logger = logger;

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ChartsResponse>> Charts()
        {
            var response = new ChartsResponse()
            {
                Totals = await _dashboardView.GetTotals(),
                Revenue = await _dashboardView.RevenueTotals(),
                Categories = await _dashboardView.CategoryRatios(),
                CategoryRevenue = await _dashboardView.CategoryRevenue(),
                PendingShipments = await _dashboardView.PendingShipments(),
                CategoryHeatmap = await _dashboardView.CategoryHeatmap()
            };

            return response;
        }
    }
}
