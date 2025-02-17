namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class CarrierNotFoundException : Exception
    {
        public CarrierNotFoundException() { }
        public CarrierNotFoundException(string msg) : base(msg) { }
        public CarrierNotFoundException(string msg, Exception inner) : base(msg, inner) { }
    }
}
