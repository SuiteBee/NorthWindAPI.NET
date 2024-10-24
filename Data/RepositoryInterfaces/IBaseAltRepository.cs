namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface IBaseAltRepository<T>
    {
        public Task<IEnumerable<T>> ReturnEntityListAsync();
        public Task<T?> FindEntityAsync(string id);
        public Task<T> AddEntityAsync(T entity);
        public Task<bool> RemoveEntityAsync(string id);
        public Task<bool> RemoveDependentEntityAsync(string id);
        public Task<T?> UpdateEntityAsync(string id, T entity);
    }
}
