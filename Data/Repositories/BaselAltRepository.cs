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
            return entity;
        }

        public async Task<IEnumerable<T>> AddMultipleEntitiesAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        public async Task RemoveEntityAsync(string id)
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
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T?> UpdateEntityAsync(string id, T entity)
        {
            if (id != entity.Id)
            {
                return null;
            }

            _dbSet.Entry(entity).State = EntityState.Modified;

            return await _dbSet.FindAsync(id);
        }
    }
}
