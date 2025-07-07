namespace Models.DTOs;

public class GameListDto {
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required List<GameDto> Games { get; set; } = new();

    public required List<PersonDto> People { get; set; } = new();

    public required DateTime CreationDate { get; set; }

    public GameDto? RandomlyPickedGame { get; set; } = null;
}