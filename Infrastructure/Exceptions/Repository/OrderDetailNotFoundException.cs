namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class OrderDetailNotFoundException : Exception
    {
        public OrderDetailNotFoundException() { }
        public OrderDetailNotFoundException(string msg) : base(msg) { }
        public OrderDetailNotFoundException(string msg, Exception inner) : base(msg, inner) { }
    }
}
