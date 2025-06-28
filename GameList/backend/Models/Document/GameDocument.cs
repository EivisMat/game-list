using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Models.Document;

public class GameDocument {
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("additionDate")]
    public required DateTime AdditionDate { get; set; }

    [BsonElement("owners")]
    public required Dictionary<Guid, bool> Owners { get; set; }

    [BsonElement("isExcluded")]
    public required bool IsExcluded { get; set; }
}