using Microsoft.EntityFrameworkCore;
using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.Repositories
{
    public class BaseAltRepository<T> : IBaseAltRepository<T> where T : EntityAlt
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseAltRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> ReturnEntityListAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> FindEntityAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> AddEntityAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        /// <summary>
        /// Delete entity and save changes to database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> RemoveEntityAsync(string id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }


            return true;
        }

        /// <summary>
        /// Deletes entity and does not save the changes. Call this method to delete all dependencies and finish with RemoveEntityAsync()
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> RemoveDependentEntityAsync(string id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            try
            {
                _dbSet.Remove(entity);
            }
            catch
            {
                return false;
            }


            return true;
        }

        public async Task<T?> UpdateEntityAsync(string id, T entity)
        {
            if (id != entity.Id)
            {
                return null;
            }

            _dbSet.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _dbSet.AnyAsync(x => x.Id == id))
                {
                    return entity;
                }
                else
                {
                    throw;
                }
            }

            return await _dbSet.FindAsync(id);
        }

    }
}
