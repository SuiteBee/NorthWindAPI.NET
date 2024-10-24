namespace NorthWindAPI.Controllers.Models.Requests
{
    public class ShipRequest
    {
        public required List<int> OrderIds { get; set; }
        public string? ShipDate { get; set; }
    }
}
