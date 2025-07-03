using AutoMapper;
using Models.Domain;
using Models.Document;

public class GameListService : IGameListService {
    private readonly IGameListRepository _repository;
    private readonly ISecurityService _securityService;
    private readonly IMapper _mapper;

    public GameListService(IGameListRepository repository, ISecurityService securityService, IMapper mapper) {
        _repository = repository;
        _securityService = securityService;
        _mapper = mapper;
    }

    public async Task CreateListAsync(GameList gameList) {
        // Check that game list doesn't already exist
        if (await _repository.GetByNameAsync(gameList.Name) is not null
            || await _repository.GetByIdAsync(gameList.Id) is not null) {
            throw new InvalidOperationException("Game list already exists.");
        }

        foreach (Game game in gameList.Games) {
            foreach (Person person in gameList.People) {
                game.AddOwner(person);
            }
        }

        gameList.Password = _securityService.HashPassword(gameList.Password);

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

        gameList.AddGame(game);

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

        gameList.RemoveGame(gameId);

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

        gameList.AddPerson(person);

        gameListDocument = _mapper.Map<GameListDocument>(gameList);

        await _repository.UpdateAsync(gameListDocument);

        return gameList;
    }

    public async Task<GameList> RemovePersonAsync(Guid listId, Guid personId) {
        GameListDocument? gameListDocument = await _repository.GetByIdAsync(listId);

        // Check list exists
        if (gameListDocument is null) {
            throw new InvalidOperationException("Game list doesn't exist.");
        }

        GameList gameList = _mapper.Map<GameList>(gameListDocument);

        gameList.RemovePerson(personId);

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

        gameList.SetGameExclusion(gameId, isExcluded);

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

        gameList.SetGameOwnership(gameId, personId, isOwner);

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
        if (gameListDocument is null) {
            throw new InvalidOperationException("Game list doesn't exist.");
        }

        GameList gameList = _mapper.Map<GameList>(gameListDocument);

        Game? game = gameList.PickRandomGame();

        gameListDocument = _mapper.Map<GameListDocument>(gameList);

        await _repository.UpdateAsync(gameListDocument);

        return game;
    }

    public async Task<GameList> GetListByIdAsync(Guid listId) {
        GameListDocument? gameListDocument = await _repository.GetByIdAsync(listId);

        // Check list exists
        if (gameListDocument is null) {
            throw new InvalidOperationException("Game list doesn't exist.");
        }

        return _mapper.Map<GameList>(gameListDocument);
    }
    public async Task<GameList> GetListByNameAsync(string listName) {
        GameListDocument? gameListDocument = await _repository.GetByNameAsync(listName);

        // Check list exists
        if (gameListDocument is null) {
            throw new InvalidOperationException("Game list doesn't exist.");
        }

        return _mapper.Map<GameList>(gameListDocument);
    }
}