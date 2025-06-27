namespace GameList.Config;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public int ExpiryDays { get; set; } = 31;
    
    public JwtSettings() {
        Secret = string.Empty;
        ExpiryDays = 31;
    }

    public JwtSettings(string Secret, int ExpiryDays) {
        this.Secret = Secret;
        this.ExpiryDays = ExpiryDays;
    }
}
