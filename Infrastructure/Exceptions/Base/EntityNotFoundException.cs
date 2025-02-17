namespace NorthWindAPI.Infrastructure.Exceptions.Base
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() { }
        public EntityNotFoundException(string msg) : base(msg) { }
        public EntityNotFoundException(string msg, Exception inner) : base(msg, inner) { }
    }
}
