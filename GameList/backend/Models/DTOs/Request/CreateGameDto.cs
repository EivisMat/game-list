namespace Models.DTOs;

public class CreateGameDto {
    public string Name { get; set; } = string.Empty;

    public DateTime AdditionDate { get; set; } = DateTime.UtcNow;

    public Dictionary<Guid, string> Owners { get; set; } = new();
}