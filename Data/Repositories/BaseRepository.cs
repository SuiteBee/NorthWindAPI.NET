using Microsoft.EntityFrameworkCore;
using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> ReturnEntityListAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> FindEntityAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> AddEntityAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<T>> AddMultipleEntitiesAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return entities;
        }

        /// <summary>
        /// Delete entity and save changes to database
        /// </summary>
        /// <param name="id"></param>
        public async Task<bool> RemoveEntityAsync(int id)
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
        public async Task<bool> RemoveDependentEntityAsync(int id)
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
                return true;
            }

            return true;
        }

        public async Task<T?> UpdateEntityAsync(int id, T entity)
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
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await _dbSet.AnyAsync(x => x.Id == id))
                {
                    return entity;
                }
                else
                {
                    throw new Exception(ex.Message + " : Error updating entity");
                }
            }

            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Updates multiple entities before saving because EntityFramework doesn't handle many single updates well
        /// </summary>
        /// <param name="entities"></param>
        public async Task UpdateMultipleEntityAsync(IEnumerable<T> entities)
        {
            var entityIdCollection = entities.Select(x => x.Id);
            var entitiesToUpdate = _dbSet.Where(x => entityIdCollection.Contains(x.Id)).ToList();
            entitiesToUpdate.ForEach(x => _dbSet.Entry(x).State = EntityState.Modified);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception(ex.Message + " : Error updating multiple entity");
            }
        }
    }
}
