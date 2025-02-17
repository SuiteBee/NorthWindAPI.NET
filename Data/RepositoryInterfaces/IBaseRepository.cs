namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface IBaseRepository<T> where T : class
    {
        public Task<IEnumerable<T>> ReturnEntityListAsync();
        public Task<T> FindEntityAsync(int id);
        public T AddEntity(T entity);
        public Task RemoveEntityAsync(int id);
        public T UpdateEntity(int id, T entity);
        public IEnumerable<T> UpdateMultipleEntity(IEnumerable<T> entities);

    }
}
