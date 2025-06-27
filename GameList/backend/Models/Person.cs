using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameList.Models;

public class Person {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    
    public Person() {
        Id = ObjectId.GenerateNewId().ToString();
        Name = string.Empty;
    }

    public Person(string Name) {
        Id = ObjectId.GenerateNewId().ToString();
        this.Name = Name;
    }
}