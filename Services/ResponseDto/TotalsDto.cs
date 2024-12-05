namespace NorthWindAPI.Services.ResponseDto
{
    public class TotalsDto
    {
        public int NumOrders { get; set; }
        public decimal RevenueTotal { get; set; }
        public int NumClients { get; set; }
        public int NumProducts { get; set; }
        public int ProductsSold { get; set; }
        public int Countries { get; set; }
    }
}
