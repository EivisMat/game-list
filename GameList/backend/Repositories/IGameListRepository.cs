using Models.Document;

public interface IGameListRepository {
    Task CreateAsync(GameListDocument gameList);

    Task<GameListDocument?> GetByIdAsync(Guid name);

    Task<GameListDocument?> GetByNameAsync(string name);

    Task<bool> UpdateAsync(GameListDocument gameList);

    Task<bool> DeleteByIdAsync(Guid id);
}