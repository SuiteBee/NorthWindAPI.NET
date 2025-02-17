namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class SupplierNotFoundException : Exception
    {
        public SupplierNotFoundException() { }
        public SupplierNotFoundException(string msg) : base(msg) { }
        public SupplierNotFoundException(string msg, Exception inner) : base(msg, inner) { }
    }
}
