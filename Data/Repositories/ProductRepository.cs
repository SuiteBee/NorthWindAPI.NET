using Microsoft.EntityFrameworkCore;
using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Infrastructure.Exceptions.Base;
using NorthWindAPI.Infrastructure.Exceptions.Repository;

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
            try
            {
                return await _baseProductRepo.ReturnEntityListAsync();
            }
            catch (EntityNotFoundException ex)
            {
                throw new ProductNotFoundException($"Products {ex.Message}");
            }
        }

        public async Task<IEnumerable<Category>> AllCategories()
        {
            var categories = await _context.Category.ToListAsync();
            if(categories == null || categories.Count == 0)
            {
                throw new ProductCategoryNotFoundException($"Categories not found");
            }
            return categories;
        }

        public async Task<IEnumerable<Supplier>> AllSuppliers()
        {
            var suppliers = await _context.Supplier.ToListAsync();
            if (suppliers == null || suppliers.Count == 0)
            {
                throw new SupplierNotFoundException($"Suppliers not found");
            }
            return suppliers;
        }

        #endregion

        #region " GET "

        public async Task<Product> FindProduct(int productId)
        {
            var product = QueryProducts().Where(x => x.Id == productId);
            if(product == null || !product.Any())
            {
                throw new ProductNotFoundException($"Product {productId} not found");
            }
            return await product.FirstAsync();
        }

        public async Task<Category> FindCategory(int categoryId)
        {
            var category = QueryCategories().Where(x => x.Id == categoryId);
            if(category == null || !category.Any())
            {
                throw new ProductCategoryNotFoundException($"Product category {categoryId} not found");
            }
            return await category.FirstAsync();
        }

        public async Task<Supplier> FindSupplier(int supplierId)
        {
            var supplier = QuerySuppliers().Where(x => x.Id == supplierId);
            if(supplier == null || !supplier.Any())
            {
                throw new SupplierNotFoundException($"Supplier {supplierId} not found");
            }
            return await supplier.FirstAsync();
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

        #region " PUT "

        public Product UpdateProduct(int id, Product prod)
        {
            try
            {
                return _baseProductRepo.UpdateEntity(id, prod);
            }
            catch (EntityNotUpdatedException ex)
            {
                throw new ProductNotUpdatedException($"Product {ex.Message}");
            }
        }

        public void UpdateMultipleProducts(IEnumerable<Product> prods)
        {
            try
            {
                _baseProductRepo.UpdateMultipleEntity(prods);
            }
            catch (EntityNotUpdatedException ex)
            {
                throw new ProductNotUpdatedException($"Products {ex.Message}");
            }
        }

        #endregion

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
