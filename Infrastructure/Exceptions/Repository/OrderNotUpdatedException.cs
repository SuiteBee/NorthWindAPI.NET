namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class OrderNotUpdatedException : Exception
    {
        public OrderNotUpdatedException() { }
        public OrderNotUpdatedException(string msg) : base(msg) { }
        public OrderNotUpdatedException(string msg, Exception inner) : base(msg, inner) { }
    }
}
