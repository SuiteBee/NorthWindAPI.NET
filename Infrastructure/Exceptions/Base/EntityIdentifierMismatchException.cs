namespace NorthWindAPI.Infrastructure.Exceptions.Base
{
    public class EntityIdentifierMismatchException : Exception
    {
        public EntityIdentifierMismatchException() { }
        public EntityIdentifierMismatchException(string msg) : base(msg) { }
        public EntityIdentifierMismatchException(string msg, Exception inner) : base(msg, inner) { }
    }
}
