namespace Models.DTOs;

public class LoginResponseDto {
    public required string Id { get; set; } = string.Empty;
    public required string AccessToken { get; set; } = string.Empty;
}