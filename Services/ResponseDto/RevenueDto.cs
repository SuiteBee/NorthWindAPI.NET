namespace NorthWindAPI.Services.ResponseDto
{
    public class RevenueDto
    {
        public string? Year { get; set; }
        public decimal Total { get; set; }
        public decimal QuarterOne { get; set; }
        public decimal QuarterTwo { get; set; }
        public decimal QuarterThree { get; set; }
        public decimal QuarterFour { get; set; }
    }
}
