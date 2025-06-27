using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using GameList.Config;
using Microsoft.Extensions.Options;

namespace GameList.Services
{
    public class SecurityService
    {
        private readonly string _jwtSecret;
        private readonly TimeSpan _jwtExpiry;
        
        public SecurityService(IOptions<JwtSettings> settings) {
            _jwtSecret = settings.Value.Secret;
            _jwtExpiry = TimeSpan.FromDays(settings.Value.ExpiryDays);
        }

        // Hash a plain-text password
        public string HashPassword(string password) {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Verify a plain-text password against a hashed one
        public bool VerifyPassword(string hashedPassword, string inputPassword) {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }

        // Generate JWT token based on a list ID
        public string GenerateJwtToken(string listId) {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_jwtSecret);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, listId)
                }),
                Expires = DateTime.UtcNow.Add(_jwtExpiry),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Validate JWT token and return list ID if valid, null if invalid
        public string? ValidateJwtToken(string token) {
            if (token == null) return null;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_jwtSecret);

            try {
                tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken.Claims.First(x => x.Type == "nameid").Value;
            }
            catch {
                return null;
            }
        }
    }
}
