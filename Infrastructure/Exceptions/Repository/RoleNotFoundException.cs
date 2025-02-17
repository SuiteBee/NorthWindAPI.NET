namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class RoleNotFoundException : Exception
    {
        public RoleNotFoundException() { }
        public RoleNotFoundException(string msg) : base(msg) { }
        public RoleNotFoundException(string msg, Exception inner) : base(msg, inner) { }
    }
}
