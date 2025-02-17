namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException() { }
        public EmployeeNotFoundException(string msg) : base(msg) { }
        public EmployeeNotFoundException(string msg, Exception inner) : base(msg, inner) { }
    }
}
