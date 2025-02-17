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
            var result = await _dbSet.AddAsync(entity);
            return result.Entity;
        }

        public async Task<IEnumerable<T>> AddMultipleEntitiesAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        public async Task RemoveEntityAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new Exception($"{id} does not exist.");
            }

            try
            {
                _dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T?> UpdateEntityAsync(int id, T entity)
        {
            if (id != entity.Id)
            {
                return null;
            }

            _dbSet.Entry(entity).State = EntityState.Modified;

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
        }
    }
}
