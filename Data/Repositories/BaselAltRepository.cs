using Microsoft.EntityFrameworkCore;
using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Infrastructure.Exceptions.Base;

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
            var entities = await _dbSet.ToListAsync();
            if (entities == null || entities.Count == 0)
            {
                throw new EntityNotFoundException($"not found.");
            }
            return entities;
        }

        public async Task<T> FindEntityAsync(string id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
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

        public IEnumerable<T> AddMultipleEntities(IEnumerable<T> entities)
        {
            try
            {
                _dbSet.AddRange(entities);
                return entities;
            }
            catch(Exception ex)
            {
                throw new EntityNotCreatedException($"not added: {ex.Message}");
            }
        }

        public async Task RemoveEntityAsync(string id)
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

        public T UpdateEntity(string id, T entity)
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
            catch (Exception ex)
            {
                throw new EntityNotUpdatedException($"not updated: {ex.Message}");
            }
        }
    }
}
