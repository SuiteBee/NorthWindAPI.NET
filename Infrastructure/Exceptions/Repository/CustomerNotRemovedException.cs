namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class CustomerNotRemovedException : Exception
    {
        public CustomerNotRemovedException() { }
        public CustomerNotRemovedException(string msg) : base(msg) { }
        public CustomerNotRemovedException(string msg, Exception inner) : base(msg, inner) { }
    }
}
