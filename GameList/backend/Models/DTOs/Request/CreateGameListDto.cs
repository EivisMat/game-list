namespace Models.DTOs;

public class CreateGameListDto {
    public string Name { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public List<CreateNamedEntityDto> Games { get; set; } = new();

    public List<CreateNamedEntityDto> People { get; set; } = new();
}