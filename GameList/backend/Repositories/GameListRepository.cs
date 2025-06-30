using Models.Document;
using MongoDB.Driver;

public class GameListRepository : IGameListRepository {

    private readonly IMongoCollection<GameListDocument> _collection;

    public GameListRepository(IMongoDatabase database) {
        _collection = database.GetCollection<GameListDocument>("gameLists");
    }

    public async Task CreateAsync(GameListDocument gameList) {
        await _collection.InsertOneAsync(gameList);
    }

    public async Task<GameListDocument?> GetByIdAsync(Guid id) {
        return await _collection.Find(g => g.Id == id).FirstOrDefaultAsync();
    }

    public async Task<GameListDocument?> GetByNameAsync(string name) {
        return await _collection.Find(g => g.Name == name).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(GameListDocument gameList) {
        var result = await _collection.ReplaceOneAsync(g => g.Id == gameList.Id, gameList);

        return result.IsAcknowledged && result.MatchedCount > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id) {
        var result = await _collection.DeleteOneAsync(g => g.Id == id);

        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}