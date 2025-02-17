namespace NorthWindAPI.Infrastructure.Exceptions.Base
{
    public class EntityNotCreatedException : Exception
    {
        public EntityNotCreatedException() { }
        public EntityNotCreatedException(string msg) : base(msg) { }
        public EntityNotCreatedException(string msg, Exception inner) : base(msg, inner) { }
    }
}
