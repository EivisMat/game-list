using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Models.Domain;

public class PersonDocument {
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }
}