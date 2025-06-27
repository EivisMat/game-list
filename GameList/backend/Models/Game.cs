using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace GameList.Models;

public class Game
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime AdditionDate { get; set; }
    public Dictionary<string, bool> Owners { get; set; }
    public bool IsExcluded { get; set; }

    public Game() {
        this.Id = ObjectId.GenerateNewId().ToString();
        this.Name = string.Empty;
        this.AdditionDate = DateTime.Now;
        this.Owners = new Dictionary<string, bool>();
        this.IsExcluded = false;
    }

    public Game(string Name) {
        this.Id = ObjectId.GenerateNewId().ToString();
        this.Name = Name;
        this.AdditionDate = DateTime.Now;
        this.Owners = new Dictionary<string, bool>();
        this.IsExcluded = false;
    }

    public Game(string Name, Dictionary<string, bool> Owners) {
        this.Id = ObjectId.GenerateNewId().ToString();
        this.Name = Name;
        this.AdditionDate = DateTime.Now;
        this.Owners = Owners;
        this.IsExcluded = false;
    }

    public Game(string Name, Dictionary<string, bool> Owners, bool IsExcluded) {
        this.Id = ObjectId.GenerateNewId().ToString();
        this.Name = Name;
        this.AdditionDate = DateTime.Now;
        this.Owners = Owners;
        this.IsExcluded = IsExcluded;
    }

    public void AddOwner(string Id, bool IsOwner) {
        if (!Owners.ContainsKey(Id)) {
            Owners.Add(Id, IsOwner);
        }
        else {
            Owners[Id] = IsOwner;
        }
    }

    public void RemoveOwner(string Id) {
        Owners.Remove(Id);
    }

    public void ToggleOwner(string Id, bool state) {
        Owners[Id] = state;
    }

    public void ToggleExclude(bool state) {
        IsExcluded = state;
    }
}