namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class OrderDetailNotCreatedException : Exception
    {
        public OrderDetailNotCreatedException() { }
        public OrderDetailNotCreatedException(string msg) : base(msg) { }
        public OrderDetailNotCreatedException(string msg, Exception inner) : base(msg, inner) { }
    }
}
