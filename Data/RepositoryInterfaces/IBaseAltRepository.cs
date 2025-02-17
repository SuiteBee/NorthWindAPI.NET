namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface IBaseAltRepository<T>
    {
        public Task<IEnumerable<T>> ReturnEntityListAsync();
        public Task<T> FindEntityAsync(string id);
        public T AddEntity(T entity);
        public IEnumerable<T> AddMultipleEntities(IEnumerable<T> entities);
        public Task RemoveEntityAsync(string id);
        public T UpdateEntity(string id, T entity);
    }
}
