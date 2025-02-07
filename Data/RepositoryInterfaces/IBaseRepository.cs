namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface IBaseRepository<T> where T : class
    {
        public Task<IEnumerable<T>> ReturnEntityListAsync();
        public Task<T?> FindEntityAsync(int id);
        public Task<T> AddEntityAsync(T entity);
        public Task<bool> RemoveEntityAsync(int id);
        public Task<bool> RemoveDependentEntityAsync(int id);
        public Task<T?> UpdateEntityAsync(int id, T entity);
        public Task UpdateMultipleEntityAsync(IEnumerable<T> entities);

    }
}
