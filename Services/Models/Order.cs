namespace NorthWindAPI.Services.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? OrderDate { get; set; }
        public required Customer OrderedBy { get; set; }
        public required Employee CompletedBy { get; set; }
        public required Shipping SendTo { get; set; }

    }
}
