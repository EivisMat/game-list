namespace Models.Domain;

public class GameList {
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

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
    }

    public void RemovePerson(Person person) {
        Person? FoundPerson = _people.FirstOrDefault(p => p.Id == person.Id);
        if (FoundPerson is not null) {
            _people.Remove(FoundPerson);
        }
        else {
            throw new InvalidOperationException("Person doesn't exist in the list.");
        }
    }

    public void AddGame(Game game) {
        if (_games.Any(g => g.Id == game.Id)) {
            throw new InvalidOperationException("Game already exists.");
        }

        _games.Add(game);
    }

    public void RemoveGame(Game game) {
        Game? FoundGame = _games.FirstOrDefault(g => g.Id == game.Id);
        if (FoundGame is not null) {
            _games.Remove(FoundGame);
        }
        else {
            throw new InvalidOperationException("Game doesn't exist in the list.");
        }
    }

    public void PickRandomGame() {
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
    }
}