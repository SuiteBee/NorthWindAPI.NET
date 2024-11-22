using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers.Models.Responses
{
    public class TotalResponse
    {
        public IEnumerable<TotalsDto> Revenue { get; set; } = new List<TotalsDto>();
        public IEnumerable<CategoryTotalsDto> Categories { get; set; } = new List<CategoryTotalsDto>();
    }
}
