using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameList.Models;

public class GameList
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public List<Game> Games { get; set; }
    public List<Person> People { get; set; }
    public Game? RandomlyPickedGame { get; set; }

    public GameList() {
        Name = string.Empty;
        Password = string.Empty;
        Games = new List<Game>();
        People = new List<Person>();
        RandomlyPickedGame = null;
    }

    public GameList(string name, string password) {
        Name = name;
        Password = password;
        Games = new List<Game>();
        People = new List<Person>();
        RandomlyPickedGame = null;
    }

    public GameList(string name, string password, List<Game> games) {
        Name = name;
        Password = password;
        Games = games;
        People = new List<Person>();
        RandomlyPickedGame = null;
        GetRandomGame();
    }
    
    public GameList(string name, string password, List<Game> games, List<Person> people) {
        Name = name;
        Password = password;
        Games = games;
        People = people;
        RandomlyPickedGame = null;
        GetRandomGame();
    }

    public GameList(string name, string password, List<Game> games, List<Person> people, Game randomGame) {
        Name = name;
        Password = password;
        Games = games;
        People = people;
        RandomlyPickedGame = randomGame;
    }

    public void AddGame(Game game) {
        Games.Add(game);
    }

    public void RemoveGame(Game game) {
        Games.Remove(game);
    }

    public Game GetRandomGame() {
        List<Game> availableGames = Games
            .Where(g => g != null 
                        && !g.IsExcluded 
                        && g.Name != RandomlyPickedGame?.Name
                        && g.Owners != null 
                        && g.Owners.All(owner => owner.Value))
            .ToList();

        if (availableGames.Count == 0) {
            return null;
        }

        Random random = new Random();
        int index = random.Next(availableGames.Count);
        RandomlyPickedGame = availableGames[index];
        return RandomlyPickedGame;
    }
}
