using GameList.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GameList.Services;

public class GameListsService {
    private readonly IMongoCollection<Models.GameList> _listsCollection;

    public GameListsService(IOptions<GameListDatabaseSettings> dbSettings) {
        var client = new MongoClient(dbSettings.Value.ConnectionString);
        var database = client.GetDatabase(dbSettings.Value.DatabaseName);
        _listsCollection = database.GetCollection<Models.GameList>(dbSettings.Value.GamesCollectionName);
    }

    // Get a list by name
    public async Task<Models.GameList?> GetByNameAsync(string name) =>
        await _listsCollection.Find(l => l.Name == name).FirstOrDefaultAsync();

    // Get a list by ID (primary key)
    public async Task<Models.GameList?> GetByIdAsync(string id) =>
        await _listsCollection.Find(l => l.Id == id).FirstOrDefaultAsync();

    // Create a new game list
    public async Task CreateAsync(Models.GameList newList) =>
        await _listsCollection.InsertOneAsync(newList);

    // Update entire game list document
    public async Task UpdateAsync(Models.GameList updatedList) =>
        await _listsCollection.ReplaceOneAsync(l => l.Id == updatedList.Id, updatedList);

    // Add a game to a list by list ID
    public async Task<bool> AddGameAsync(string listId, Game newGame) {
        var update = Builders<Models.GameList>.Update.Push(l => l.Games, newGame);
        var result = await _listsCollection.UpdateOneAsync(l => l.Id == listId, update);
        return result.ModifiedCount > 0;
    }

    // Add a person to game list by list ID
    public async Task<bool> AddPersonAsync(string listId, Person newPerson) {
        var list = await GetByIdAsync(listId);
        if (list == null) return false;

        if (list.People.Any(p => p.Name == newPerson.Name)) return false;

        list.People.Add(newPerson);

        var result = await _listsCollection.ReplaceOneAsync(l => l.Id == list.Id, list);
        return result.ModifiedCount > 0;
    }

    // Toggle ownership of a game by game ID and person name, identified by list ID
    public async Task<bool> ToggleOwnershipAsync(string listId, string gameId, string personName) {
        var list = await GetByIdAsync(listId);
        if (list == null) return false;

        var game = list.Games.FirstOrDefault(g => g.Id == gameId);
        if (game == null) return false;

        if (game.Owners.ContainsKey(personName)) {
            game.Owners[personName] = !game.Owners[personName];
        }
        else {
            game.Owners[personName] = true;
        }

        var result = await _listsCollection.ReplaceOneAsync(l => l.Id == list.Id, list);
        return result.ModifiedCount > 0;
    }

    // Remove a game by game ID from a list identified by list ID
    public async Task<bool> RemoveGameAsync(string listId, string gameId) {
        var update = Builders<Models.GameList>.Update.PullFilter(
            l => l.Games, g => g.Id == gameId);

        var result = await _listsCollection.UpdateOneAsync(l => l.Id == listId, update);
        return result.ModifiedCount > 0;
    }

    // Remove a person by name from a list identified by list ID
    public async Task<bool> RemovePersonAsync(string listId, string personName) {
        var list = await GetByIdAsync(listId);
        if (list == null) return false;

        var person = list.People.FirstOrDefault(p => p.Name == personName);
        if (person == null) return false;

        list.People.Remove(person);

        var result = await _listsCollection.ReplaceOneAsync(l => l.Id == list.Id, list);
        return result.ModifiedCount > 0;
    }

    // Delete a list by ID
    public async Task<bool> DeleteListAsync(string id) {
        var result = await _listsCollection.DeleteOneAsync(l => l.Id == id);
        return result.DeletedCount > 0;
    }
}
