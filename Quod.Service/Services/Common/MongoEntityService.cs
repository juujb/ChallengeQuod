using Quod.Domain;

namespace Quod.Service
{
    public abstract class MongoEntityService<T, TRep> : IMongoEntityService<T> 
        where T : class, IEntity
        where TRep : IMongoRepository<T>
    {
        #region Dependencies  

        private readonly TRep _repository;
        protected TRep Repository => _repository;

        #endregion Dependencies  

        #region Constructors  

        public MongoEntityService(TRep repository)
        {
            _repository = repository;
        }

        #endregion Constructors

        #region Persistence  

        public async Task AddAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            await _repository.AddAsync(entity);
        }

        #endregion Persistence  
    }
}
