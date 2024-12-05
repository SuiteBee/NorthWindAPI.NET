using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Views.Interfaces
{
    public interface IDashboardView
    {
        public Task<TotalsDto> GetTotals();
        public Task<IEnumerable<RevenueDto>> RevenueTotals();
        public Task<IEnumerable<CategoryRatiosDto>> CategoryRatios();
        public Task<IEnumerable<CategoryRevenueDto>> CategoryRevenue();
        public Task<int> PendingShipments();
        public Task<IEnumerable<CategoryHeatmapDto>> CategoryHeatmap();
    }
}
