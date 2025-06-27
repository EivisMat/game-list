namespace Models.DTOs;

public class GameDto {
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required DateTime AdditionDate { get; set; }

    public required Dictionary<Guid, bool> Owners { get; set; } = new();

    public required bool IsExcluded { get; set; }

}