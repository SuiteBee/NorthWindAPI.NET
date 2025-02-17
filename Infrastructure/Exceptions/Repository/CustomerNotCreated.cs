namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class CustomerNotCreatedException : Exception
    {
        public CustomerNotCreatedException() { }
        public CustomerNotCreatedException(string msg) : base(msg) { }
        public CustomerNotCreatedException(string msg, Exception inner) : base(msg, inner) { }
    }
}
