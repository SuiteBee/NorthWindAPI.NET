namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException() { }
        public OrderNotFoundException(string msg) : base(msg) { }
        public OrderNotFoundException(string msg, Exception inner) : base(msg, inner) { }
    }
}
