namespace NorthWindAPI.Infrastructure.Exceptions.Base
{
    public class EntityNotUpdatedException : Exception
    {
        public EntityNotUpdatedException() { }
        public EntityNotUpdatedException(string msg) : base(msg) { }
        public EntityNotUpdatedException(string msg, Exception inner) : base(msg, inner) { }
    }
}
