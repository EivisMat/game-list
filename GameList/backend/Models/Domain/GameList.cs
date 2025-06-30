namespace Models.Domain;

public class GameList {
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    private readonly List<Game> _games = new();
    public IReadOnlyList<Game> Games => _games;

    private readonly List<Person> _people = new();
    public IReadOnlyList<Person> People => _people;

    public Game? RandomlyPickedGame { get; set; } = null;

    private static readonly Random _random = new();

    public void AddPerson(Person person) {
        if (_people.Any(p => p.Id == person.Id)) {
            throw new InvalidOperationException("Person already exists.");
        }

        _people.Add(person);
        foreach (Game game in Games) {
            game.AddOwner(person);
        }
    }

    public void RemovePerson(Guid personId) {
        Person? FoundPerson = _people.FirstOrDefault(p => p.Id == personId);
        if (FoundPerson is null) {
            throw new InvalidOperationException("Person doesn't exist in the list.");
        }
        _people.Remove(FoundPerson);
        foreach (Game game in Games) {
            game.RemoveOwner(personId);
        }
    }

    public void AddGame(Game game) {
        if (_games.Any(g => g.Id == game.Id)) {
            throw new InvalidOperationException("Game already exists.");
        }

        _games.Add(game);
    }

    public void RemoveGame(Guid gameId) {
        Game? FoundGame = _games.FirstOrDefault(g => g.Id == gameId);
        if (FoundGame is null) {
            throw new InvalidOperationException("Game doesn't exist in the list.");
        }

        _games.Remove(FoundGame);
    }

    public Game? PickRandomGame() {
        List<Game> EligibleGames = _games
                                .Where(g => g.Owners.All(o => o.Value) &&
                                            (RandomlyPickedGame == null || g.Id != RandomlyPickedGame.Id))
                                .ToList();

        if (EligibleGames.Count == 0) {
            this.RandomlyPickedGame = null;
        }
        else {
            this.RandomlyPickedGame = EligibleGames[_random.Next(EligibleGames.Count)];
        }
        return RandomlyPickedGame;
    }

    public void SetGameExclusion(Guid gameId, bool isExcluded) {
        Game? FoundGame = _games.FirstOrDefault(g => g.Id == gameId);

        if (FoundGame is null) {
            throw new InvalidOperationException("Game doesn't exist.");
        }

        FoundGame.SetExclusion(isExcluded);
    }

    public void SetGameOwnership(Guid gameId, Guid personId, bool isOwner) {
        Game? FoundGame = _games.FirstOrDefault(g => g.Id == gameId);

        if (FoundGame is null) {
            throw new InvalidOperationException("Game doesn't exist.");
        }

        FoundGame.SetOwner(personId, isOwner);
    }
}