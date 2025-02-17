namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() { }
        public UserNotFoundException(string msg) : base(msg) { }
        public UserNotFoundException(string msg, Exception inner) : base(msg, inner) { }
    }
}
