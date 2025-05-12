using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Quod.Domain;

namespace Quod.Infra.Mongo
{
    public abstract class MongoRepository<T> : IMongoRepository<T>
            where T : class, IEntity
    {
        protected readonly IMongoCollection<T> _collection;
        protected readonly IMongoDatabase _mongoDatabase;
        public virtual IMongoCollection<T> Collection { get { return _collection; } }
        public virtual string CollectionName { get; init; } = string.Empty;
        public MongoCollectionSettings CollectionSettings { get; protected set; } = new MongoCollectionSettings();
        protected virtual Action<T> OnBeforeSave { get => (entity) => { }; }

        protected MongoRepository(IOptions<DefaultMongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<T>(CollectionName, this.CollectionSettings);
            _mongoDatabase = database;
        }

        public async Task AddAsync(T entity)
        {
            OnBeforeSave(entity);
            await _collection.InsertOneAsync(entity);
        }
    }
}
