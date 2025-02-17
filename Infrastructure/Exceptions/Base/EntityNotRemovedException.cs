namespace NorthWindAPI.Infrastructure.Exceptions.Base
{
    public class EntityNotRemovedException : Exception
    {
        public EntityNotRemovedException() { }
        public EntityNotRemovedException(string msg) : base(msg) { }
        public EntityNotRemovedException(string msg, Exception inner) : base(msg, inner) { }
    }
}
