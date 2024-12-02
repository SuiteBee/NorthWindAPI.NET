using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> AllProducts();
        public Task<IEnumerable<Category>> AllCategories();
        public Task<IEnumerable<Supplier>> AllSuppliers();
        public Task<Product> FindProduct(int productId);
        public Task<Category> FindCategory(int categoryId);
        public Task<Supplier> FindSupplier(int supplierId);
        public Task<Product> UpdateProduct(int id, Product prod);
    }
}
