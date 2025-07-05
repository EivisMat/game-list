using Models.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Document;

public class GameListDocument {
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("password")]
    public required string Password { get; set; }

    [BsonElement("creationDate")]
    public required DateTime CreationDate { get; set; }

    [BsonElement("games")]
    public required List<GameDocument> Games { get; set; }

    [BsonElement("people")]
    public required List<PersonDocument> People { get; set; }

    [BsonElement("randomlyPickedGame")]
    public GameDocument? RandomlyPickedGame { get; set; }
}