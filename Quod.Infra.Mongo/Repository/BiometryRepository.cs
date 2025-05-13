using Microsoft.Extensions.Options;
using Quod.Domain;

namespace Quod.Infra.Mongo
{
    public class BiometryRepository : MongoRepository<Biometry>, IBiometryRepository
    {
        public override string CollectionName => MongoCollections.Biometry;

        public BiometryRepository(IOptions<DefaultMongoDbSettings> settings) : base(settings)
        {

        }
    }
}
