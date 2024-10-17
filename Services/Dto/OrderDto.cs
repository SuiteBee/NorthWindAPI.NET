namespace NorthWindAPI.Services.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string? OrderDate { get; set; }
        public ProductDto? Product { get; set; }
        public CustomerDto? OrderedBy { get; set; }
        public EmployeeDto? CompletedBy { get; set; }
        public  ShippingDto? SendTo { get; set; }

    }
}
