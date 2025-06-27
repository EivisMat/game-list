namespace Models.DTOs;

public class LoginResponseDto {
    public required string AccessToken { get; set; } = string.Empty;

    public required string RefreshToken { get; set; } = string.Empty;
}