using Models.Domain;

public interface IGameListService {
    Task CreateListAsync(GameList gameList);

    Task<GameList> AddGameAsync(Guid listId, Game game);
    Task<GameList> RemoveGameAsync(Guid listId, Guid gameId);

    Task<GameList> AddPersonAsync(Guid listId, Person person);
    Task<GameList> RemovePersonAsync(Guid listId, Guid personId);

    Task<GameList> SetGameExclusionAsync(Guid listId, Guid gameId, bool isExcluded);
    Task<GameList> SetGameOwnershipAsync(Guid listId, Guid gameId, Guid personId, bool isOwner);

    Task<bool> DeleteListAsync(Guid listId);

    Task<Game?> PickRandomGameAsync(Guid listId);

    Task<GameList?> GetListByIdAsync(Guid id);
    Task<GameList?> GetListByNameAsync(string name);
}