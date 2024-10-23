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

        public async Task AddEntityAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveEntityAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return -1;
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<T?> UpdateEntityAsync(int id, T entity)
        {
            if( id != entity.Id)
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
                if(!await _dbSet.AnyAsync(x => x.Id == id))
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
