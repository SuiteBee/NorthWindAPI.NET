namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface IBaseAltRepository<T>
    {
        public Task<IEnumerable<T>> ReturnEntityListAsync();
        public Task<T?> FindEntityAsync(string id);
        public Task AddEntityAsync(T entity);
        public Task<string?> RemoveEntityAsync(string id);
        public Task<T?> UpdateEntityAsync(string id, T entity);
    }
}
