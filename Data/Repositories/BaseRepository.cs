using Microsoft.EntityFrameworkCore;
using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        private readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        private DbSet<T> DbSet
        {
            get { return _context.Set<T>(); }
        }

        public async Task<IEnumerable<T>> ReturnEntityListAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<T?> FindEntityAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task AddEntityAsync(T entity)
        {
            await DbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveEntityAsync(int id)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity == null)
            {
                return -1;
            }

            DbSet.Remove(entity);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<T?> UpdateEntityAsync(int id, T entity)
        {
            if( id != entity.Id)
            {
                return null;
            }

            DbSet.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!await DbSet.AnyAsync(x => x.Id == id))
                {
                    return entity;
                }
                else
                {
                    throw;
                }
            }

            return await DbSet.FindAsync(id);
        }
    }
}
