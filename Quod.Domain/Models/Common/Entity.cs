using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Quod.Domain
{
    public class Entity : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("deletado")]
        public bool Deleted { get; set; } = false;
    }
}
