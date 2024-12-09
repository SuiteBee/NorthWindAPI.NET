using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers.Models.Responses
{
    public class RegionsResponse
    {
        public IEnumerable<RegionDto> Regions { get; set; } = new List<RegionDto>();
    }
}
