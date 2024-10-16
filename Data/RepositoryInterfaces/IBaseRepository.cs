namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface IBaseRepository<T> where T : class
    {
        public Task<IEnumerable<T>> ReturnEntityListAsync();
        public Task<T?> FindEntityAsync(int id);
        public Task AddEntityAsync(T entity);
        public Task<int> RemoveEntityAsync(int id);
        public Task<T?> UpdateEntityAsync(int id, T entity);

    }
}
