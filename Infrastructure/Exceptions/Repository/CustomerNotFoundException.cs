namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException() { }
        public CustomerNotFoundException(string msg) : base(msg) { }
        public CustomerNotFoundException(string msg, Exception inner) : base(msg, inner) { }
    }
}
