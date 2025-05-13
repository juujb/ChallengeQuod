using Quod.Domain;

namespace Quod.Service
{
    public class BiometryEntityService : MongoEntityService<Biometry, IBiometryRepository>, IBiometryEntityService
    {
        public BiometryEntityService(IBiometryRepository repository) : base(repository)
        {

        }

    }
}
