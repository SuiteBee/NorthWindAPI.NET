namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class ProductNotUpdatedException : Exception
    {
        public ProductNotUpdatedException() { }
        public ProductNotUpdatedException(string msg) : base(msg) { }
        public ProductNotUpdatedException(string msg, Exception inner) : base(msg, inner) { }
    }
}
