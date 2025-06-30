using AutoMapper;
using Models.Domain;
using Models.Document;
using System.Diagnostics;

public class GameListService : IGameListService {

    private readonly IGameListRepository _repository;
    private readonly IMapper _mapper;

    public GameListService(IGameListRepository repository, IMapper mapper) {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task CreateListAsync(GameList gameList) {
        // Check that game list doesn't already exist
        if (await _repository.GetByNameAsync(gameList.Name) is not null
            || await _repository.GetByIdAsync(gameList.Id) is not null) {
            throw new InvalidOperationException("Game list already exists.");
        }

        // Map to GameListDocument
        GameListDocument gameListDocument = _mapper.Map<GameListDocument>(gameList);

        await _repository.CreateAsync(gameListDocument);
    }

    public async Task<GameList> AddGameAsync(Guid listId, Game game) {
        GameListDocument? gameListDocument = await _repository.GetByIdAsync(listId);

        // Check that game list exists
        if (gameListDocument is null) {
            throw new InvalidOperationException("Game list doesn't exist.");
        }

        GameList gameList = _mapper.Map<GameList>(gameListDocument);

        // Try to add game. If it fails, should throw exception - pass that onto the controller
        try {
            gameList.AddGame(game);
        }
        catch {
            throw;
        }

        // Map back to document
        gameListDocument = _mapper.Map<GameListDocument>(gameList);

        await _repository.UpdateAsync(gameListDocument);

        return gameList;
    }

    public async Task<GameList> RemoveGameAsync(Guid listId, Guid gameId) {
        GameListDocument? gameListDocument = await _repository.GetByIdAsync(listId);

        // Check list exists
        if (gameListDocument is null) {
            throw new InvalidOperationException("Game list doesn't exist.");
        }

        GameList gameList = _mapper.Map<GameList>(gameListDocument);

        // Try to remove game. If it fails, should throw exception - pass that onto the controller
        try {
            gameList.RemoveGame(gameId);
        }
        catch {
            throw;
        }

        gameListDocument = _mapper.Map<GameListDocument>(gameList);

        await _repository.UpdateAsync(gameListDocument);

        return gameList;
    }

    public async Task<GameList> AddPersonAsync(Guid listId, Person person) {
        GameListDocument? gameListDocument = await _repository.GetByIdAsync(listId);

        // Check list exists
        if (gameListDocument is null) {
            throw new InvalidOperationException("Game list doesn't exist.");
        }

        GameList gameList = _mapper.Map<GameList>(gameListDocument);

        // Try to add person
        try {
            gameList.AddPerson(person);
        }
        catch {
            throw;
        }

        gameListDocument = _mapper.Map<GameListDocument>(gameList);

        await _repository.UpdateAsync(gameListDocument);

        return gameList;
    }

    async Task<GameList> RemovePersonAsync(Guid listId, Guid personId) {
        GameListDocument? gameListDocument = await _repository.GetByIdAsync(listId);

        // Check list exists
        if (gameListDocument is null) {
            throw new InvalidOperationException("Game list doesn't exist.");
        }

        GameList gameList = _mapper.Map<GameList>(gameListDocument);

        // Try to remove person
        try {
            gameList.RemovePerson(personId);
        }
        catch {
            throw;
        }

        gameListDocument = _mapper.Map<GameListDocument>(gameList);

        await _repository.UpdateAsync(gameListDocument);

        return gameList;
    }

    public async Task<GameList> SetGameExclusionAsync(Guid listId, Guid gameId, bool isExcluded) {
        GameListDocument? gameListDocument = await _repository.GetByIdAsync(listId);

        // Check list exists
        if (gameListDocument is null) {
            throw new InvalidOperationException("Game list doesn't exist.");
        }

        GameList gameList = _mapper.Map<GameList>(gameListDocument);

        // Try to set exclusion
        try {
            gameList.SetGameExclusion(gameId, isExcluded);
        }

        gameListDocument = _mapper.Map<GameListDocument>(gameList);

        await _repository.UpdateAsync(gameListDocument);

        return gameList;
    }
    public async Task<GameList> SetGameOwnershipAsync(Guid listId, Guid gameId, Guid personId, bool isOwner) {
        GameListDocument? gameListDocument = await _repository.GetByIdAsync(listId);

        // Check list exists
        if (gameListDocument is null) {
            throw new InvalidOperationException("Game list doesn't exist.");
        }

        GameList gameList = _mapper.Map<GameList>(gameListDocument);

        // Try to set ownership
        try {
            gameList.SetGameOwnership(gameId, personId, isOwner);
        }

        gameListDocument = _mapper.Map<GameListDocument>(gameList);

        await _repository.UpdateAsync(gameListDocument);

        return gameList;
    }

    public async Task<bool> DeleteListAsync(Guid listId) {
        return await _repository.DeleteByIdAsync(listId);
    }

    public async Task<Game?> PickRandomGameAsync(Guid listId) {
        GameListDocument? gameListDocument = await _repository.GetByIdAsync(listId);

        // Check list exists
        if (gameListDocument is not null) {
            throw new InvalidOperationException("Game list doesn't exist.");
        }

        GameList gameList = _mapper.Map<GameList>(gameListDocument);

        Game? game = gameList.PickRandomGame();

        gameListDocument = _mapper.Map<GameListDocument>(gameList);

        return game;
    }

    public async Task<GameList?> GetListByIdAsync(Guid listId) {
        GameListDocument? gameListDocument = await _repository.GetByIdAsync(listId);

        // Check list exists
        if (gameListDocument is not null) {
            throw new InvalidOperationException("Game list doesn't exist.");
        }

        return _mapper.Map<GameList>(gameListDocument);
    }
    public async Task<GameList?> GetListByNameAsync(string listName) {
        GameListDocument? gameListDocument = await _repository.GetByNameAsync(listName);

        // Check list exists
        if (gameListDocument is not null) {
            throw new InvalidOperationException("Game list doesn't exist.");
        }

        return _mapper.Map<GameList>(gameListDocument);
    }
}