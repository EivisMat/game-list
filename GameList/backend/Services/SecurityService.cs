using System.Security.Claims;
using AutoMapper;
using BCrypt.Net;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class SecurityService : ISecurityService {
    private readonly JwtSettings _jwtSettings;

    public SecurityService(IOptions<JwtSettings> jwtSettings) {
        _jwtSettings = jwtSettings.Value;
    }

    public string EncryptPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public bool ValidatePassword(string password, string hashedPassword) {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    public string CreateAccessToken(Guid listId) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

        var claims = new[]
        {
            new Claim("listId", listId.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public bool ValidateAccessToken(string token) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

        try {
            tokenHandler.ValidateToken(token, new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return true;
        }
        catch {
            return false;
        }
    }

}