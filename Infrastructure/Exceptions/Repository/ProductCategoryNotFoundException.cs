namespace NorthWindAPI.Infrastructure.Exceptions.Repository
{
    public class ProductCategoryNotFoundException : Exception
    {
        public ProductCategoryNotFoundException() { }
        public ProductCategoryNotFoundException(string msg) : base(msg) { }
        public ProductCategoryNotFoundException(string msg, Exception inner) : base(msg, inner) { }
    }
}
