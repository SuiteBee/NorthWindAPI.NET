using Microsoft.EntityFrameworkCore;
using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Infrastructure.Exceptions.Base;

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
            var entities = await _dbSet.ToListAsync();
            if(entities == null || entities.Count == 0)
            {
                throw new EntityNotFoundException($"not found.");
            }
            return entities;
        }

        public async Task<T> FindEntityAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if(entity == null)
            {
                throw new EntityNotFoundException($"{id} not found.");
            }
            return entity; 
        }

        public T AddEntity(T entity)
        {
            try
            {
                var added = _dbSet.Add(entity);
                return added.Entity;
            }
            catch (Exception ex)
            {
                throw new EntityNotCreatedException($"not added: {ex.Message}");
            }
        }

        public async Task RemoveEntityAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException($"{id} not found.");
            }

            try
            {
                _dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                throw new EntityNotRemovedException($"not deleted: {ex.Message}");
            }
        }

        public T UpdateEntity(int id, T entity)
        {
            if (id != entity.Id)
            {
                throw new EntityIdentifierMismatchException($"ID does not match the ID in the request body.");
            }

            try
            {
                _dbSet.Entry(entity).State = EntityState.Modified;

                return _dbSet.Entry(entity).Entity;
            }
            catch(Exception ex)
            {
                throw new EntityNotUpdatedException($"not updated: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates multiple entities before saving because EntityFramework doesn't handle many single updates well
        /// </summary>
        /// <param name="entities"></param>
        public IEnumerable<T> UpdateMultipleEntity(IEnumerable<T> entities)
        {
            try
            {
                var entityIdCollection = entities.Select(x => x.Id);
                var entitiesToUpdate = _dbSet.Where(x => entityIdCollection.Contains(x.Id)).ToList();
                entitiesToUpdate.ForEach(x => _dbSet.Entry(x).State = EntityState.Modified);

                return _dbSet.Where(x => entityIdCollection.Contains(x.Id)).ToList();
            }
            catch (Exception ex)
            {
                throw new EntityNotUpdatedException($"not updated: {ex.Message}");
            }
        }
    }
}
