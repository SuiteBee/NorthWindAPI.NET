using Microsoft.EntityFrameworkCore;
using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IBaseRepository<Product> _baseProductRepo;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
            _baseProductRepo = new BaseRepository<Product>(_context);
        }

        #region " GET ALL "

        public async Task<IEnumerable<Product>> AllProducts()
        {
            return await _baseProductRepo.ReturnEntityListAsync();
        }

        public async Task<IEnumerable<Category>> AllCategories()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<IEnumerable<Supplier>> AllSuppliers()
        {
            return await _context.Supplier.ToListAsync();
        }

        #endregion

        #region " GET "

        public async Task<Product> FindProduct(int productId)
        {
            return await QueryProducts().Where(x => x.Id == productId).FirstAsync();
        }

        public async Task<Category> FindCategory(int categoryId)
        {
            return await QueryCategories().Where(x => x.Id == categoryId).FirstAsync();
        }

        public async Task<Supplier> FindSupplier(int supplierId)
        {
            return await QuerySuppliers().Where(x => x.Id == supplierId).FirstAsync();
        }

        private IQueryable<Product> QueryProducts()
        {
            return _context.Product.AsQueryable();
        }

        private IQueryable<Category> QueryCategories()
        {
            return _context.Category.AsQueryable();
        }

        private IQueryable<Supplier> QuerySuppliers()
        {
            return _context.Supplier.AsQueryable();
        }

        #endregion
    }
}
