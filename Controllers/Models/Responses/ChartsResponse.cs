using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers.Models.Responses
{
    public class ChartsResponse
    {
        public TotalsDto Totals { get; set; } = new TotalsDto();
        public IEnumerable<RevenueDto> Revenue { get; set; } = new List<RevenueDto>();
        public IEnumerable<CategoryRatiosDto> Categories { get; set; } = new List<CategoryRatiosDto>();
        public IEnumerable<CategoryRevenueDto> CategoryRevenue { get; set; } = new List<CategoryRevenueDto>();
        /// <summary>
        /// Count for orders not yet marked as shipped
        /// </summary>
        /// <example>99</example>
        public int PendingShipments { get; set; }
        public IEnumerable<CategoryHeatmapDto> CategoryHeatmap { get; set; } = new List<CategoryHeatmapDto>();

    }
}
