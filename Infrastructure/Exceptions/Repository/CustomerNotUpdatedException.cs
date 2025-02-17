namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class CustomerNotUpdatedException : Exception
    {
        public CustomerNotUpdatedException() { }
        public CustomerNotUpdatedException(string msg) : base(msg) { }
        public CustomerNotUpdatedException(string msg, Exception inner) : base(msg, inner) { }
    }
}
