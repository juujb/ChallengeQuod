namespace Quod.Infra.Mongo
{
    public class DefaultMongoDbSettings : IMongoDatabaseSettings
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
    }
}