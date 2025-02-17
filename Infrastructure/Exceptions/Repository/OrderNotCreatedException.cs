namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class OrderNotCreatedException : Exception
    {
        public OrderNotCreatedException() { }
        public OrderNotCreatedException(string msg) : base(msg) { }
        public OrderNotCreatedException(string msg, Exception inner) : base(msg, inner) { }
    }
}
