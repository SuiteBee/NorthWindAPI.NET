namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class UserNotUpdatedException : Exception
    {
        public UserNotUpdatedException() { }
        public UserNotUpdatedException(string msg) : base(msg) { }
        public UserNotUpdatedException(string msg, Exception inner) : base(msg, inner) { }
    }
}
