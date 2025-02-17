namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class OrderNotRemovedException : Exception
    {
        public OrderNotRemovedException() { }
        public OrderNotRemovedException(string msg) : base(msg) { }
        public OrderNotRemovedException(string msg, Exception inner) : base(msg, inner) { }
    }
}
