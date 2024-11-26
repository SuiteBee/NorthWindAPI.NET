﻿using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers.Models.Responses
{
    public class TotalResponse
    {
        public IEnumerable<RevenueDto> Revenue { get; set; } = new List<RevenueDto>();
        public IEnumerable<CategoryRatiosDto> Categories { get; set; } = new List<CategoryRatiosDto>();
        public IEnumerable<CategoryRevenueDto> CategoryRevenue { get; set; } = new List<CategoryRevenueDto>();
        public int PendingShipments { get; set; }

    }
}
