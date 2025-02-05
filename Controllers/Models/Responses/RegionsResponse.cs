using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers.Models.Responses
{
    public class RegionsResponse
    {
        /// <summary>
        /// List of global regions
        /// </summary>
        /// <example>Central America,Western Europe,British Isles</example>
        public IEnumerable<RegionDto> Regions { get; set; } = new List<RegionDto>();
    }
}
