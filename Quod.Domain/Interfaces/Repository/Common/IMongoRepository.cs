namespace Quod.Domain
{
    public interface IMongoRepository<T> where T : class, IEntity
    {
        Task AddAsync(T entity);
    }
}
