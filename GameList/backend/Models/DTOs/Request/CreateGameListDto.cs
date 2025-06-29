namespace Models.DTOs;

public class CreateGameListDto {
    public string Name { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public List<CreateGameDto> Games { get; set; } = new();

    public List<CreatePersonDto> People { get; set; } = new();
}