namespace Models.Domain;

public class Game {
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public DateTime AdditionDate { get; set; } = DateTime.UtcNow;

    public Dictionary<Guid, bool> Owners { get; set; } = new();

    public bool IsExcluded { get; set; } = false;
}