using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers.Models.Responses
{
    public class ShipOptionResponse
    {
        public IEnumerable<CarrierDto> Carriers { get; set; } = new List<CarrierDto>();
    }
}
