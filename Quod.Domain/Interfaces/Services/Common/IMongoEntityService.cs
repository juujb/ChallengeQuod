namespace Quod.Domain
{
    public interface IMongoEntityService<T> where T : IEntity
    {
        Task AddAsync(T entity);
    }
}
